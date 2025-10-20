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
        string cacheKey = $"{nameof(GetArtistByIdQueryHandler)}-{request.Id}"; 
        var cachedArtists = await cache.GetRecordAsync<ArtistByIdVm>(cacheKey);

        if (cachedArtists == null)
        {
            var artistId = new ArtistId(request.Id);
        
            var artistResult = await artistRepository.GetByIdAsync(artistId);
        
            if(artistResult.IsFailure) 
            {
                throw new CustomNotFoundException(nameof(Artist), request.Id);
            }
        
            var entity = artistResult.Value; 

            var resultVm = mapper.Map<ArtistByIdVm>(entity);
            
            await cache.SetRecordAsync(cacheKey, resultVm, TimeSpan.FromHours(1));

            return resultVm;
        }
        else 
        {
            return cachedArtists; 
        }
    }
}