using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.RemoveTrackFromFavorites;

public class RemoveTrackFromFavoritesCommand : IRequest<Result<Guid, Error>>
{
    // Ідентифікатор користувача, який додає трек
    public UserId UserId { get; set; } 

    // Ідентифікатор треку, який додається
    public TrackId TrackId { get; set; }
}