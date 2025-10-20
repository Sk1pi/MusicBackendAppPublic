using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;
using MusicBackendApp.Infrastructure.DataBase;

namespace MusicBackendApp.Infrastructure.Persistence.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly DbContextAccess _contextAccess;
    private readonly ILogger<ArtistRepository> _logger;

    public ArtistRepository(
        DbContextAccess contextAccess,
        ILogger<ArtistRepository> logger)
    {
        _contextAccess = contextAccess;
        _logger = logger;
    }
    
    public async Task<Result<Artist, Error>> GetByIdAsync(ArtistId  id)
    {
        var artists = await _contextAccess.Artists
            .FirstOrDefaultAsync(x => x.Id == id); 
        if(artists == null)
            return Result.Failure<Artist, Error>(Errors.General.NotFound(id.ToString()));
        
        return Result.Success<Artist, Error>(artists);
    }
    
    public async Task<Result<Artist, Error>> FindExactByNameAsync(ArtistName name)
    {
        var artist = await _contextAccess.Artists
            .FirstOrDefaultAsync(a => a.ArtistName.Value == name.Value);

        return artist is not null
            ? Result.Success<Artist, Error>(artist)
            : Result.Failure<Artist, Error>(Errors.General.NotFound());
    }

    public async Task<Artist?> FindByUserIdAsync(UserId userId)
    {
        return await _contextAccess.Artists
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<IEnumerable<Artist>> SearchByNameAsync(ArtistName name)
    {
        return await _contextAccess.Artists
            .Where(a => a.ArtistName.Value.Contains(name.Value))
            .ToListAsync();
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsAsync(int count)
    {
        return await _contextAccess.Artists
            .OrderByDescending(a => a.Subs) 
            .Take(count)                   
            .ToListAsync();
    }

    public async Task AddAsync(Artist artist)
    {
        await _contextAccess.Artists.AddAsync(artist);
    }

    public void Delete(Artist artist)
    {
        _contextAccess.Artists.Remove(artist);
    }

    public async Task<List<Artist>> GetAllAsync()
    {
        return await _contextAccess.Artists.ToListAsync(); 
    }
}