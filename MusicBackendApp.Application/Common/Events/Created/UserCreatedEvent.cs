namespace MusicBackendApp.Application.Common.Events.Created;

public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}