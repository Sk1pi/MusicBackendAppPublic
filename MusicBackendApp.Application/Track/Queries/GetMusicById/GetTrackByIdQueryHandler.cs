using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Application.Common.Extensions;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Track.Queries.GetMusicById;

public class GetTrackByIdQueryHandler
: IRequestHandler<GetTrackByIdQuery, TrackIdVm>
{
    private readonly ITrackRepository _trackRepository;
    private IMapper _mapper;
    private readonly IDistributedCache _cache;

    public GetTrackByIdQueryHandler(
        ITrackRepository trackRepository,
        IMapper mapper,
        IDistributedCache cache) => 
        (_trackRepository, _mapper, _cache) = (trackRepository, mapper, cache);

    public async Task<TrackIdVm> Handle(
        GetTrackByIdQuery request, 
        CancellationToken cancellationToken)
    {
        string cacheKey = $"{nameof(GetTrackByIdQueryHandler)}-{request.Id}"; //Може бути помилкою, бо має бути інший код
        var cachedTrack = await _cache.GetRecordAsync<TrackIdVm>(cacheKey);

        if (cachedTrack is null)
        {
            // ▼▼▼ Створюємо Value Object з Guid, що прийшов у запиті ▼▼▼
            var trackId = new TrackId(request.Id);
        
            var trackResult = await _trackRepository.GetByIdAsync(trackId);

            if (trackResult.IsFailure) //Виправити помилку
            {
                throw new CustomNotFoundException(nameof(Track), request.Id);
            }
        
            var entity = trackResult.Value;
            
            var mappedEntity = _mapper.Map<TrackIdVm>(entity);
            
            await _cache.SetRecordAsync(cacheKey, mappedEntity, TimeSpan.FromSeconds(60));

            return mappedEntity;
        }
        else
        {
            return cachedTrack;
        }
    }
}