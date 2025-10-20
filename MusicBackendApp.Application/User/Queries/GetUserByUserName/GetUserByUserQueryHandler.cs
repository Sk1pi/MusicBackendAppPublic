using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Application.User.Queries.GetUserByUserName;

public class GetUserByUserQueryHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IDistributedCache cache) : IRequestHandler<GetUserByIUserQuery, UserListVm>
{
    public async Task<UserListVm> Handle(GetUserByIUserQuery request, 
        CancellationToken cancellationToken)
    {
        var cacheKey = $"GetUserByName{request.Name}";
        var cachedUser = await cache.GetRecordAsync<List<UserLookupDto>>(cacheKey);

        if (cachedUser is null)
        {
            var userNameResult = UserName.Create(request.Name);

            if (userNameResult.IsFailure)
            {
                return new UserListVm { Users = new List<UserLookupDto>() };
            }
        
            var artistName = userNameResult.Value; // Отримуємо колекцію Artists
        
            var userResult = await userRepository.GetByName(artistName);
            if (userResult.IsFailure)
            {
                return new UserListVm { Users = new List<UserLookupDto>() };
            }

            var entities = userResult.Value;
        
            var users = entities.AsQueryable() // Перетворюємо IEnumerable на IQueryable, якщо потрібно для ProjectTo
             .ProjectTo<UserLookupDto>(mapper.ConfigurationProvider)
             .ToListAsync(cancellationToken);
        
            var mappedUsers = await users; // Очікуємо результат мапінгу
        
            await cache.SetRecordAsync(cacheKey, mappedUsers, TimeSpan.FromHours(1));
            
            return new UserListVm {Users = mappedUsers};
        }
            
        return new UserListVm { Users = cachedUser };
    }
}