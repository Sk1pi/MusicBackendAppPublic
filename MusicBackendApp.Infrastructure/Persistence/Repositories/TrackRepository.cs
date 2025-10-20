using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;
using MusicBackendApp.Infrastructure.DataBase;

namespace MusicBackendApp.Infrastructure.Persistence.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly DbContextAccess _contextAccess;

    public TrackRepository(DbContextAccess contextAccess)
    {
        _contextAccess = contextAccess;
    }
    
    public async Task<Result<Track, Error>> GetByIdAsync(TrackId id)
    {
        var tracks = await _contextAccess.Tracks
            .FirstOrDefaultAsync(x => x.Id == id);
        return tracks is not null
            ? Result.Success<Track, Error>(tracks)
            : Result.Failure<Track, Error>(Errors.General.NotFound());
    }

    public async Task<bool> DoesArtistHaveTrackWithTitleAsync(ArtistId artistId, Title title)
    {
        return await _contextAccess.Tracks
            .AnyAsync(t => t.ArtistId == artistId && t.Title.Value == title.Value);
    }

    public async Task<bool> DoesExistWithTitleAsync(Title title)
    {
        return await _contextAccess.Tracks
            .AnyAsync(x => x.Title.Value == title.Value);
    }

    public async Task<Result<IQueryable<Track>, Error>> GetByTitle(Title title) 
    {
        var query = _contextAccess.Tracks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title.Value))
        {
            query = query.Where(x => x.Title!.Value.Contains(title.Value));
        }
        
        return Result.Success<IQueryable<Track>, Error>(query);
    }
    
    public async Task<Result<IEnumerable<Track>, Error>> GetByArtistIdAsync(Guid artistId)
    {
        var tracks = _contextAccess.Tracks
            .Where(x => x.Id.Value == artistId); 
        
        if(tracks == null || !tracks.Any())
            return Result.Failure<IEnumerable<Track>, Error>(Errors.General.NotFound($"Tracks for artist {artistId}")); 
        
        return Result.Success<IEnumerable<Track>, Error>(tracks);
    }

    public async Task AddAsync(Track track)
    {
        await _contextAccess.Tracks.AddAsync(track);
    }

    public void DeleteAsync(Track track)
    {
        _contextAccess.Tracks.Remove(track);
    }

    public async Task<IEnumerable<Track>> SearchByTitleAsync(Title title)
    {
        return await _contextAccess.Tracks
            .Include(t => t.Author) 
            .Where(t => t.Title.Value.Contains(title.Value))
            .ToListAsync();
    }

    public async Task<List<Track>> GetAllAsync()
    {
        return await _contextAccess.Tracks.ToListAsync();
    }
}