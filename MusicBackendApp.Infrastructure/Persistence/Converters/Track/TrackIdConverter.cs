using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Track;

/*public class TrackIdConverter : ValueConverter<TrackId, Guid>
{
    public TrackIdConverter()
        : base(
            id => id.Value,
            value => TrackId.FromGuid(value).Value)
    {}
}*/