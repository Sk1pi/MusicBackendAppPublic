using MusicBackendApp.Application.Artists.Queries.GetArtistByName;
using MediatR;

namespace MusicBackendApp.Application.Artists.Queries.GetTopArtist;


// Для простоти можна використовувати record
public record GetTopArtistsQuery(int Count = 10) : IRequest<ArtistListVm>;