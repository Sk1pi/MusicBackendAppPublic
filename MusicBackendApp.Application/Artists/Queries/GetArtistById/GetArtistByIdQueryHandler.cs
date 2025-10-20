using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistById;

public class GetArtistByIdQueryHandler(
    IArtistRepository artistRepository,
    IMapper mapper,
    IDistributedCache cache) : IRequestHandler<GetArtistByIdQuery, ArtistByIdVm>
{
    public async Task<ArtistByIdVm> Handle(GetArtistByIdQuery request, 
        CancellationToken cancellationToken)
    {
        string cacheKey = $"{nameof(GetArtistByIdQueryHandler)}-{request.Id}"; //Обережно, бо тут мав бути інший код
        var cachedArtists = await cache.GetRecordAsync<ArtistByIdVm>(cacheKey);

        if (cachedArtists == null)
        {
            var artistId = new ArtistId(request.Id);
        
            var artistResult = await artistRepository.GetByIdAsync(artistId);
        
            if(artistResult.IsFailure) //Виправити помилку
            {
                // Або можна адаптувати, щоб він приймав ваш Error об'єкт.
                throw new CustomNotFoundException(nameof(Artist), request.Id);
                // Або, якщо ваш Error має тип "NotFound", можна так:
                // if (artistResult.Error.Type == ErrorType.NotFound) throw new CustomNotFoundException(artistResult.Error.Message);
            }
        
            var entity = artistResult.Value; // Отримуємо саму сутність Artist

            var resultVm = mapper.Map<ArtistByIdVm>(entity);
            
            await cache.SetRecordAsync(cacheKey, resultVm, TimeSpan.FromHours(1));

            return resultVm;
        }
        else // 6. Якщо дані в кеші Є (Cache Hit)
        {
            return cachedArtists; // Просто повертаємо закешовані дані
        }
    }
    
    /*src.Id.Value: У AutoMapper, src.Id є типом ArtistId. Але ArtistByIdVm.Id очікує Guid.
     Тому ви використовуєте .Value для "розпакування" Guid з ArtistId.
     Те ж саме стосується ArtistName.Value, Email.Value, Password.Value.*/
    /*(entity): Це джерельний об'єкт, який потрібно перетворити.
     У цьому випадку, entity – це ваш об'єкт доменної сутності Artist, який ви отримали з репозиторію.*/
}