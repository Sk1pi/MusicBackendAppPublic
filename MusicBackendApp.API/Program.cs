using System.Reflection;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MusicBackendApp.Application;
using MusicBackendApp.Application.Common.Consumers.AddRemove;
using MusicBackendApp.Application.Common.Consumers.Created;
using MusicBackendApp.Application.Common.Consumers.Indexer;
using MusicBackendApp.Application.Common.Consumers.Login;
using MusicBackendApp.Application.Common.Interfaces.Auth;
using MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;
using MusicBackendApp.Application.Common.Interfaces.Jwt;
using MusicBackendApp.Application.Common.Interfaces.Permission;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Common.Interfaces.Search;
using MusicBackendApp.Application.Common.Interfaces.Services;
using MusicBackendApp.Application.Factories.UserSubscriptions;
using MusicBackendApp.Application.Track.Builders;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Extensions;
using MusicBackendApp.Infrastructure;
using MusicBackendApp.Infrastructure.AuthificationAuthorization;
using MusicBackendApp.Infrastructure.AuthificationAuthorization.Jwt;
using MusicBackendApp.Infrastructure.Configurations.RolePermission.Services;
using MusicBackendApp.Infrastructure.Configurations.Search;
using MusicBackendApp.Infrastructure.Configurations.Search.Services;
using MusicBackendApp.Infrastructure.Configurations.Services;
using MusicBackendApp.Infrastructure.Persistence.Repositories;
using MusicBackendApp.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "MusicApp_";
});

builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticSettings"));
builder.Services.AddScoped<ISearchService, ElasticSearchService>(); // Змінено на AddScoped, оскільки ElasticSearchService залежить від Scoped-сервісів (DbContext)

builder.Services.AddHangfire(config => 
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ArtistCreatedConsumer>();
    x.AddConsumer<TrackCreatedConsumer>();
    x.AddConsumer<UsersCreatedConsumer>();
    x.AddConsumer<TrackAddedToFavoritesConsumer>();
    x.AddConsumer<TrackRemovedFromFavoritesConsumer>();
    x.AddConsumer<UserLoginConsumer>();
    x.AddConsumer<ArtistIndexerConsumer>();
    x.AddConsumer<TrackIndexerConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"), c =>
        {
            c.Username("guest"); 
            c.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient<IPaymentGatewayService, PaymentGatewayService>();

builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddHttpClient<IStudentCardVerificationService, StudentCardVerificationService>();
builder.Services.AddScoped<PremiumSubscriptionFactory>();
builder.Services.AddScoped<FamilySubscriptionFactory>();
builder.Services.AddScoped<StudentSubscriptionFactory>();

builder.Services.AddTransient<ITrackBuilder, TrackBuilder>();
builder.Services.AddTransient<TrackDirector>();

builder.Services.AppApiAuthorization(builder.Configuration);
builder.Services.AddPermissionAuthorization();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Music Backend API",
        Description = "An ASP.NET Core Web API for managing a music library.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Name",
            Url = new Uri("https://your-contact-url.com")
        }
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicBackendApp.API v1");
        options.RoutePrefix = string.Empty; 
    });
}

app.UseCustomExceptionHandler(); 


app.UseStaticFiles();
app.UseCors("AllowMyFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();
    searchService.CreateIndexIfNotExistsAsync(builder.Configuration.GetSection("ElasticSettings:DefaultIndex").Value).Wait();
    
    RecurringJob.AddOrUpdate<ISearchService>(
        "ReindexAllData",
        x => x.ReindexAllDataAsync(),
        Cron.Daily());
}

app.Run();