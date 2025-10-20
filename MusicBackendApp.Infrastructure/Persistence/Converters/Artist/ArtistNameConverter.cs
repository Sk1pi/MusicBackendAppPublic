using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Artist;

public class ArtistNameConverter : ValueConverter<ArtistName, string>
{
    public ArtistNameConverter()
        : base(
            name => name.Value, 
            value => ArtistName.Create(value).Value) 
    { }
}