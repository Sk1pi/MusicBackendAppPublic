using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.User;

public class UserNameConverter : ValueConverter<UserName, string>
{
    public UserNameConverter()
        : base(
            name => name.Value,
            value => UserName.Create(value).Value)
    {}
}