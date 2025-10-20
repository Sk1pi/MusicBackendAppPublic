using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistByName;

public class GetArtistByNameQueryHandler 
    : IRequestHandler<GetArtistByNameQuery, ArtistListVm>
{
    private readonly IDistributedCache _cache;
    private readonly IArtistRepository _artistRepository;
    private readonly IMapper _mapper;
    
    public GetArtistByNameQueryHandler(IArtistRepository artistRepository, 
        IMapper mapper, IDistributedCache cache) =>
        (_artistRepository, _mapper, _cache) = (artistRepository, mapper, cache);

    public async Task<ArtistListVm> Handle(GetArtistByNameQuery request, 
        CancellationToken cancellationToken)
    {
        var cacheKey = $"GetArtistByNameQueryHandler_{request.Name}";
        var cachedArtist = await _cache.GetRecordAsync<List<ArtistLookupDto>>(cacheKey);

        if (cachedArtist == null)
        {
            var nameResult = ArtistName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Якщо назва невалідна, повертаємо порожній список
                return new ArtistListVm { Artists = new List<ArtistLookupDto>() };
            }
        
            var artistName = nameResult.Value;
        
            var artistsFromDb = await _artistRepository.SearchByNameAsync(artistName);
        
            var mappedArtists = _mapper.Map<List<ArtistLookupDto>>(artistsFromDb);
            
            await _cache.SetRecordAsync(cacheKey, mappedArtists, TimeSpan.FromDays(1));
        
            return new ArtistListVm {Artists = mappedArtists};
        }
        
        return new ArtistListVm { Artists = cachedArtist };
    }
}