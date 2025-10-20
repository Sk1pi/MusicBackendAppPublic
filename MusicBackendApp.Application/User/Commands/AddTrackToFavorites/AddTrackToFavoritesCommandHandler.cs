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
        
        // 3. Додати трек до улюблених треків користувача (бізнес-логіка на рівні домену)
        // Примітка: Цей метод має бути доданий до твоєї сутності User
        // Він також повинен перевіряти, чи трек вже не в списку улюблених
        var addResult = user.AddFavoriteTrack(trackToAdd); // Припустимо, ти створиш цей метод у класі User

        if (addResult.IsFailure)
        {
            return Result.Failure<Guid, Error>(Errors.General.NotFound("Can`t add track to favorites"));
        }
        
        // 4. Зберегти зміни в репозиторії (і таким чином у базі даних)
        // Тобі, можливо, доведеться додати метод UpdateAsync(User user) в IUserRepository
        await _userRepository.UpdateUserAsync(user); // Або SaveChangesAsync() якщо це DbContext
        
        // 5. Інвалідувати кеш для улюблених треків цього користувача
        string cacheKey = $"FavoriteTracks_UserId_{request.UserId.Value}";
        await _cache.RemoveAsync(cacheKey);

        await _publishEndpoint.Publish(new TrackAddedToFavoritesEvent
        {
            UserId = request.UserId.Value,
            TrackId = trackToAdd.Id.Value
        });

        // 6. Повернути успішний результат (можна повернути ID треку, який був доданий)
        return Result.Success<Guid, Error>(request.TrackId.Value);
    }
}