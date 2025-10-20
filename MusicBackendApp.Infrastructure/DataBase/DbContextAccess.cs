using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Application.Common.Interfaces; // Цей using, можливо, не потрібен, якщо DbContextAccess не реалізує IDbContextAccess напряму
using MusicBackendApp.Domain.Entites; // Переконайтеся, що Entites -> Entities, якщо це опечатка
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicBackendApp.Domain.Entites.Subscriptions;
using MusicBackendApp.Infrastructure.Configurations.RolePermission.Configurations;

namespace MusicBackendApp.Infrastructure.DataBase;

public class DbContextAccess(
    DbContextOptions<DbContextAccess> options,
    IOptions<AuthorizationOptions> authorizationOptions): DbContext(options), IDbContextAccess
{

    public DbSet<Track> Tracks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<FamilySubscription> FamilySubscriptions { get; set; }
    public DbSet<PremiumSubscription> PremiumSubscriptions { get; set; }
    public DbSet<StudentSubscription> StudentSubscriptions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(CreateLoggerFactory()) 
            .EnableSensitiveDataLogging(); 
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authorizationOptions.Value));
        
        base.OnModelCreating(modelBuilder);
    }
    
    public static ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); }); 
}
