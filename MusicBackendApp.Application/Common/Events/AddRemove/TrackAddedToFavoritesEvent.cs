namespace MusicBackendApp.Application.Common.Events;

public class TrackAddedToFavoritesEvent
{
    public Guid TrackId { get; set; }
    public Guid UserId { get; set; }
}