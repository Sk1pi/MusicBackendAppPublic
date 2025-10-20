
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration; 

namespace MusicBackendApp.Infrastructure.DataBase;

public class DbContextAccessFactory : IDesignTimeDbContextFactory<DbContextAccess>
{
    public DbContextAccess CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string not found in appsettings.json or appsettings.Development.json.");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<DbContextAccess>();
        optionsBuilder.UseNpgsql(connectionString);
        
        var authorizationOptions = Options.Create(new AuthorizationOptions());
        
        return new DbContextAccess(optionsBuilder.Options, authorizationOptions);
    }
}