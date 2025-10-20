namespace MusicBackendApp.Application.Common.Events.Created;

public class ArtistCreatedEvent
{
    public Guid ArtistId { get; set; }
    public string? ArtistName { get; set; }
}