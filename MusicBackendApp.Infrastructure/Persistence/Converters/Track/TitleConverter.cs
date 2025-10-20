using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Track;

public class TitleConverter : ValueConverter<Title, string>
{
    public TitleConverter()
        : base(
            title => title.Value,
            value => Title.Create(value).Value)
    {}
}