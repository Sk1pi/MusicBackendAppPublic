using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicBackendApp.Application.Common.Interfaces;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Infrastructure.DataBase;
using MusicBackendApp.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MusicBackendApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<DbContextAccess>(options =>
        {
            options.UseNpgsql(connectionString); 
        });
        
        services.AddScoped<IDbContextAccess>(provider => 
            provider.GetRequiredService<DbContextAccess>());
        
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}