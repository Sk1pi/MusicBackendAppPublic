
namespace MusicBackendApp.Domain.Entites.Id_s;

public readonly record struct TrackId(Guid Value)
{
    public static TrackId New() => new(Guid.NewGuid());
}