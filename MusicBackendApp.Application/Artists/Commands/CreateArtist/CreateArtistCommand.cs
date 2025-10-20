using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Artists.Commands.CreateArtist;

public class CreateArtistCommand : IRequest<Result<Guid, Error>>
{
    public string ArtistName { get; set; }
    public string Email { get; init; }
    public string Password { get; init; }

    //public IEnumerable<Guid>? TrackIds { get; set; }
    public int Subs { get; set; }
}