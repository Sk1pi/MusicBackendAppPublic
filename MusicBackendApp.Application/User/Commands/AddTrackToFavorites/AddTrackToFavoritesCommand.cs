using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.AddTrackToFavorites;

public class AddTrackToFavoritesCommand : IRequest<Result<Guid, Error>>
{
    public UserId UserId { get; set; } 
    
    public TrackId TrackId { get; set; }
}