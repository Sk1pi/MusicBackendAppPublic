
namespace MusicBackendApp.Domain.Entites.Id_s;

public readonly record struct ArtistId(Guid Value)
{
    public static ArtistId New() => new(Guid.NewGuid());
}
