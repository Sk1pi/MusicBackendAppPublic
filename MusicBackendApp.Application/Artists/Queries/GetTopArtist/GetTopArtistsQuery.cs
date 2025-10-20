using MusicBackendApp.Application.Artists.Queries.GetArtistByName;
using MediatR;

namespace MusicBackendApp.Application.Artists.Queries.GetTopArtist;

public record GetTopArtistsQuery(int Count = 10) : IRequest<ArtistListVm>;