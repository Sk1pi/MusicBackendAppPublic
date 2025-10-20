using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

public class GetTrackByTitileQueryHandler
    : IRequestHandler<GetTrackByTitileQuery, TrackListVm>
{
    private readonly ITrackRepository _trackRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public GetTrackByTitileQueryHandler(
        ITrackRepository trackRepository,
        IMapper mapper,
        IDistributedCache cache) => 
        (_trackRepository , _mapper, _cache) = (trackRepository, mapper, cache);

    public async Task<TrackListVm> Handle(GetTrackByTitileQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"GetTrackByTitle_{request.Title}";
        var cachedTrack = await _cache.GetRecordAsync<List<TrackLookupDto>>(cacheKey);

        if (cachedTrack is null)
        {
            var titleResult = Title.Create(request.Title ?? string.Empty);
            if (titleResult.IsFailure)
            {
                return new TrackListVm { Tracks = new List<TrackLookupDto>() };
            }
            
            var tracksFromDb = await _trackRepository.SearchByTitleAsync(titleResult.Value);
            
            var mappedTracks = _mapper.Map<List<TrackLookupDto>>(tracksFromDb);
            
            await _cache.SetRecordAsync(cacheKey, mappedTracks, new TimeSpan(0, 30, 0));
        
            return new TrackListVm { Tracks = mappedTracks };
        }
        
        return new TrackListVm { Tracks = cachedTrack };
    }
}