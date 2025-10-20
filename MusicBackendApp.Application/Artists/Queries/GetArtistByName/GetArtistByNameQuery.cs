using MediatR;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistByName;

public class GetArtistByNameQuery 
    : IRequest<ArtistListVm>
{
    public string Name { get; set; }
}