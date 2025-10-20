using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Infrastructure.Persistence.Converters.User;

/*public class UserIdConverter : ValueConverter<UserId, Guid>
{
    public UserIdConverter()
        : base(
            id => id.Value,
            value => UserId.FromGuid(value).Value)
    {}
}*/