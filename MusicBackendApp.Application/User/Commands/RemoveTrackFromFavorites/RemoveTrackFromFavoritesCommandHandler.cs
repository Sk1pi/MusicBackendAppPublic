using AutoMapper;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Events;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.RemoveTrackFromFavorites;

public class RemoveTrackFromFavoritesCommandHandler : IRequestHandler<RemoveTrackFromFavoritesCommand, Result<Guid, Error>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IPublishEndpoint _publishEndpoint;

    public RemoveTrackFromFavoritesCommandHandler(
        IUserRepository userRepository,
        ITrackRepository trackRepository,
        IMapper mapper,
        IDistributedCache cache,
        IPublishEndpoint publishEndpoint)
    {
        _userRepository = userRepository;
        _trackRepository = trackRepository;
        _mapper = mapper;
        _cache = cache;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Result<Guid, Error>> Handle(RemoveTrackFromFavoritesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithFavoritesAsync(request.UserId);
        if(user == null)
            return Result.Failure<Guid, Error>(Errors.General.NotFound("User"));
        
        var trackResult = await _trackRepository.GetByIdAsync(request.TrackId);
        if (trackResult.IsFailure)
            return Result.Failure<Guid, Error>(Errors.General.NotFound("Track"));
        
        var trackToDelete = trackResult.Value;
        
        var removeResult  = user.RemoveFavoriteTrack(trackToDelete.Id);
        
        if (removeResult .IsFailure)
            return Result.Failure<Guid, Error>(Errors.General.NotFound("Cannot remove track from favorites"));

        await _userRepository.UpdateUserAsync(user);
        
        string cacheKey = $"FavoriteTracks_UserId_{request.UserId.Value}";
        await _cache.RemoveAsync(cacheKey);

        await _publishEndpoint.Publish(new TrackRemovedFromFavoritesEvent
        {
            UserId = request.UserId.Value,
            TrackId = request.TrackId.Value,
        });

        // 6. Повернути успішний результат (можна повернути ID треку, який був доданий)
        return Result.Success<Guid, Error>(request.TrackId.Value);
    }
}