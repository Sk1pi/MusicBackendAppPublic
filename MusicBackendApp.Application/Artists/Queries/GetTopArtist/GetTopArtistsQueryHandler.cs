using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Artists.Queries.GetArtistByName;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;


namespace MusicBackendApp.Application.Artists.Queries.GetTopArtist;

public class GetTopArtistsQueryHandler 
    : IRequestHandler<GetTopArtistsQuery, ArtistListVm>
{
    private readonly IArtistRepository _artistRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public GetTopArtistsQueryHandler(
        IArtistRepository artistRepository, 
        IMapper mapper,
        IDistributedCache cache)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ArtistListVm> Handle(
        GetTopArtistsQuery request, 
        CancellationToken cancellationToken)
    {
        string cacheKey = $"GetTopArtists_{request.Count}";
        
        var cachedArtists = await _cache.GetRecordAsync<List<ArtistLookupDto>>(cacheKey);

        if (cachedArtists is null)
        {
            // 1. Викликаємо новий метод репозиторія
            var topArtists = await _artistRepository.GetTopArtistsAsync(request.Count);

            // 2. Мапимо результат на DTO
            var mappedArtists = _mapper.Map<List<ArtistLookupDto>>(topArtists);
            
            // 4. Збережи дані в кеші перед тим, як повернути їх
            // Наприклад, на 5 хвилин
            await _cache.SetRecordAsync(cacheKey, mappedArtists, TimeSpan.FromSeconds(60));
            
            //\Повертаємо ViewModel
            return new ArtistListVm { Artists = mappedArtists };
        }
        
        return new ArtistListVm { Artists = cachedArtists };
    }
}