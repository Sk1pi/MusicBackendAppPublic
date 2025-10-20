using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.User.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IDistributedCache cache) : IRequestHandler<GetUserByIdQuery, UserIdVm>
{
    public async Task<UserIdVm> Handle(GetUserByIdQuery request, 
        CancellationToken cancellationToken)
    {
        string cacheKey = $"GetUserById_{request.Id}";
        var cachedUser = await cache.GetRecordAsync<UserIdVm>(cacheKey);

        if (cachedUser is null)
        {
            var userId = new UserId(request.Id);
        
            var userResult = await userRepository.GetByIdAsync(userId);
        
            if(userResult.IsFailure) //Виправити помилку
            {
                throw new CustomNotFoundException(nameof(User), request.Id);
            }

            var entity = userResult.Value;
        
            var resultVm = mapper.Map<UserIdVm>(entity);
            
            await cache.SetRecordAsync(cacheKey, resultVm, TimeSpan.FromMinutes(5));
            
            return resultVm;
        }
        else
        {
            return cachedUser;
        }
        
    }
}