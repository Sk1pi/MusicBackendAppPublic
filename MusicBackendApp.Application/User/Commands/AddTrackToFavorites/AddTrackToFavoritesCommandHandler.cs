using AutoMapper;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MusicBackendApp.Application.Common.Events;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.AddTrackToFavorites;

public class AddTrackToFavoritesCommandHandler : IRequestHandler<AddTrackToFavoritesCommand, Result<Guid, Error>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ITrackRepository _trackRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddTrackToFavoritesCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IDistributedCache cache,
        ITrackRepository trackRepository,
        IPublishEndpoint publishEndpoint)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cache = cache;
        _trackRepository = trackRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Result<Guid, Error>> Handle(AddTrackToFavoritesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithFavoritesAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure<Guid, Error>(Errors.General.NotFound("User"));
        }
        
        var trackResult = await _trackRepository.GetByIdAsync(request.TrackId);
        
        if (trackResult.IsFailure)
        {
            return Result.Failure<Guid, Error>(Errors.General.NotFound("Track"));
        }
        
        var trackToAdd = trackResult.Value;
        
        var addResult = user.AddFavoriteTrack(trackToAdd); 

        if (addResult.IsFailure)
        {
            return Result.Failure<Guid, Error>(Errors.General.NotFound("Can`t add track to favorites"));
        }
        
        await _userRepository.UpdateUserAsync(user); 
        
        string cacheKey = $"FavoriteTracks_UserId_{request.UserId.Value}";
        await _cache.RemoveAsync(cacheKey);

        await _publishEndpoint.Publish(new TrackAddedToFavoritesEvent
        {
            UserId = request.UserId.Value,
            TrackId = trackToAdd.Id.Value
        });
        
        return Result.Success<Guid, Error>(request.TrackId.Value);
    }
}