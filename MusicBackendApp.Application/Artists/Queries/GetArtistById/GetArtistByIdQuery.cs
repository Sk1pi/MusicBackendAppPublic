using MediatR;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistById;

public class GetArtistByIdQuery
 : IRequest<ArtistByIdVm>
{
    //public Guid UserId { get; init; }
    public Guid Id { get; init; }
}
//ArtistByIdVm як TResponse означає, що коли GetArtistByIdQuery буде оброблений (_mediator.Send(...)), він поверне об'єкт типу ArtistByIdVm.