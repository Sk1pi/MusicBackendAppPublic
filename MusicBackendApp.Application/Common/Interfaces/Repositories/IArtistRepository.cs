using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Result<Artist, Error>> GetByIdAsync(ArtistId id);
    
    // Шукає артиста за ТОЧНИМ збігом імені. Використовується для перевірки на дублікат.
    Task<Result<Artist, Error>> FindExactByNameAsync(ArtistName name);
    
    Task<Artist?> FindByUserIdAsync(UserId userId);
    
    // Шукає ВСІХ артистів, ім'я яких містить заданий рядок. Повертає колекцію.
    Task<IEnumerable<Artist>> SearchByNameAsync(ArtistName name); //Раніше було Artist artist
    Task<IEnumerable<Artist>> GetTopArtistsAsync(int count);
    Task AddAsync(Artist artist);
    // Можливо, Task UpdateAsync(Artist artist);
    void Delete(Artist artist);
    Task<List<Artist>> GetAllAsync();
}