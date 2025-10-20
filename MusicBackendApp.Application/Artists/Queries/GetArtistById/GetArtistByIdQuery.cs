using MediatR;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistById;

public class GetArtistByIdQuery
 : IRequest<ArtistByIdVm>
{
    public Guid Id { get; init; }
}