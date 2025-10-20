using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Domain.Entites;

namespace MusicBackendApp.Application.Common.Interfaces;

public interface IDbContextAccess
{
    public DbSet<Domain.Entites.Track> Tracks { get; set; }
    public DbSet<Domain.Entites.User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}