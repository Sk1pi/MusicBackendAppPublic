using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Password;

public class PasswordConverter : ValueConverter<Domain.Entites.Objects.Passwords.Password, string>
{
    public PasswordConverter()
        : base(
            paswrd => paswrd.Value,
            value => Domain.Entites.Objects.Passwords.Password.Create(value).Value)
    {}
}