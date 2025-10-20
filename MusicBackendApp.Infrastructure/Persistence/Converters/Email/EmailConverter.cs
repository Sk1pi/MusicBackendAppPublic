using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.Email;

public class EmailConverter : ValueConverter<Domain.Entites.Objects.Email.Email, string>
{
    public EmailConverter()
        : base(
            email => email.Value,
            value => Domain.Entites.Objects.Email.Email.Create(value).Value)
    {}
}