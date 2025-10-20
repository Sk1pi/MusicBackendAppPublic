namespace MusicBackendApp.Application.Common.Events;

public class TrackRemovedFromFavoritesEvent
{
    public Guid TrackId { get; set; }
    public Guid UserId { get; set; }
}