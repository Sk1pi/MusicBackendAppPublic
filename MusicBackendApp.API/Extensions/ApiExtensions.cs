using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MusicBackendApp.Application.Common.Interfaces.Permission;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Infrastructure.AuthificationAuthorization;
using MusicBackendApp.Infrastructure.AuthificationAuthorization.Jwt;
using MusicBackendApp.Infrastructure.Configurations.RolePermission.Services;

namespace MusicBackendApp.Extensions;

public static class ApiExtensions
{
    public static void AppApiAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services
            .AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme; //
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(  
                        Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };

                options.Events =
                    new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Query["cookies"];

                            return Task.CompletedTask;
                        }
                    };
            });
        
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization();
    }
    
    public static IServiceCollection AddPermissionAuthorization(
        this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
        {
            options.AddPolicy("ReadProfilePolicy", policy =>
                policy.AddRequirements(new PermissionRequirement(
                    [Permission.Users_Read, Permission.Artists_Read])));

            options.AddPolicy("UpdateArtistPolicy", policy =>
                policy.AddRequirements(new PermissionRequirement(
                    [Permission.Users_Update, Permission.Artists_Update])));
            
            options.AddPolicy("DeleteArtistPolicy", policy =>
                policy.AddRequirements(new PermissionRequirement(
                    [Permission.Users_Delete, Permission.Artists_Delete])));
        });
    }
    
    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params Permission[] permissions)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy =>
            policy.AddRequirements(new PermissionRequirement(permissions)));
    }
}