using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Repositories;

public interface ITrackRepository
{
    Task<Result<Domain.Entites.Track, Error>> GetByIdAsync(TrackId id);
    Task<bool> DoesArtistHaveTrackWithTitleAsync(ArtistId artistId, Title title);
    Task<Result<IQueryable<Domain.Entites.Track>, Error>> GetByTitle(Title title); // було Domain.Entites.Track track
    Task<Result<IEnumerable<Domain.Entites.Track>, Error>> GetByArtistIdAsync(Guid artistId); // Новий метод, який ми обговорювали
    Task AddAsync(Domain.Entites.Track track);
    void DeleteAsync(Domain.Entites.Track track);
    
    Task<IEnumerable<Domain.Entites.Track>> SearchByTitleAsync(Title title);
    Task<List<Domain.Entites.Track>> GetAllAsync();
}
