using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Artist;

/*public class ArtistIdConverter : ValueConverter<ArtistId, Guid>
{
    public ArtistIdConverter()
        : base(
            id => id.Value, // З ArtistId на Guid
            value => ArtistId.FromGuid(value).Value) // З Guid на ArtistId (припускаємо, що FromGuid.Value завжди повертає валідний Value)
    { }
}*/