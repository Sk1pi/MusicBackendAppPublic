namespace MusicBackendApp.Application.Common.Events.Login;

public class UserLoginEvent
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}