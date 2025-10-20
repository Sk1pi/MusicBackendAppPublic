
namespace MusicBackendApp.Domain.Entites.Id_s;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
}
