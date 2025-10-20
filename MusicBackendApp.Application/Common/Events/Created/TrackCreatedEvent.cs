namespace MusicBackendApp.Application.Common.Events.Created;

public class TrackCreatedEvent
{
    public Guid TrackId { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; }
}