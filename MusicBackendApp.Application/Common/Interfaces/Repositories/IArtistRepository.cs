using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Result<Artist, Error>> GetByIdAsync(ArtistId id);
    
    Task<Result<Artist, Error>> FindExactByNameAsync(ArtistName name);
    
    Task<Artist?> FindByUserIdAsync(UserId userId);
    
    Task<IEnumerable<Artist>> SearchByNameAsync(ArtistName name); 
    Task<IEnumerable<Artist>> GetTopArtistsAsync(int count);
    Task AddAsync(Artist artist);
    void Delete(Artist artist);
    Task<List<Artist>> GetAllAsync();
}