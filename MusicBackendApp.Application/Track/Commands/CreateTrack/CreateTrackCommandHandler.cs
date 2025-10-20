using System.Security.Claims;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using MusicBackendApp.Application.Common.Events.Created;
using MusicBackendApp.Application.Common.Interfaces;
using MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Track.Builders;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Commands.CreateTrack;

public class CreateTrackCommandHandler
: IRequestHandler<CreateTrackCommand, Result<Guid, Error>>
{
    private readonly ITrackRepository _trackRepository;
    private readonly IDbContextAccess _dbContextAccess;
    private readonly IArtistRepository _artistRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITrackBuilder _trackBuilder;
    private readonly TrackDirector _trackDirector;

    public CreateTrackCommandHandler(ITrackRepository trackRepository, 
        IDbContextAccess dbContextAccess,
        IArtistRepository artistRepository,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IPublishEndpoint publishEndpoint,
        ITrackBuilder trackBuilder,
        TrackDirector trackDirector)
    {
        _trackRepository = trackRepository;
        _dbContextAccess = dbContextAccess;
        _artistRepository = artistRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _publishEndpoint = publishEndpoint;
        _trackBuilder = trackBuilder;
        _trackDirector = trackDirector;
    }
    
     public async Task<Result<Guid, Error>> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {
        // === Крок 1: Визначення користувача ===
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userIdGuid))
        {
            return Result.Failure<Guid, Error>(Errors.General.NotFound());
        }
        var userId = new UserId(userIdGuid);
        
        // === Крок 2: Пошук або створення профілю артиста ===
        var artist = await _artistRepository.FindByUserIdAsync(userId);
        
        if (artist is null)
        {
            // Метод GetByIdAsync має повертати Result<User, Error>
            var userResult = await _userRepository.GetByIdAsync(userId);
            if (userResult.IsFailure) 
            {
                return Result.Failure<Guid, Error>(userResult.Error);
            }
            var user = userResult.Value; // <-- РОЗПАКОВУЄМО РЕЗУЛЬТАТ

            var artistNameResult = ArtistName.Create(user.Name.Value); // <-- ВИКОРИСТОВУЄМО user.Name.Value
            if (artistNameResult.IsFailure) return Result.Failure<Guid, Error>(artistNameResult.Error);
            var newArtistName = artistNameResult.Value;
            
            var existingArtistResult = await _artistRepository.FindExactByNameAsync(newArtistName);
            // Перевіряємо, що ім'я не зайняте ІНШИМ користувачем
            if (existingArtistResult.IsSuccess && existingArtistResult.Value.UserId != userId)
            {
                return Result.Failure<Guid, Error>(Errors.Artist.NameAlreadyTaken(newArtistName.Value));
            }

            // Передаємо розпаковані об'єкти
            var artistCreateResult = Artist.Create(newArtistName, user.Email, user.Password, userId, user.Subs);
            if(artistCreateResult.IsFailure) return Result.Failure<Guid, Error>(artistCreateResult.Error);
            
            artist = artistCreateResult.Value;
            await _artistRepository.AddAsync(artist);

            await _publishEndpoint.Publish(new ArtistCreatedEvent
            {
                ArtistId = userId.Value,
                ArtistName = newArtistName.Value,
            });
        }
        
        // === Крок 3: Валідація та створення треку ===
        var titleResult = Title.Create(request.Title);
        if (titleResult.IsFailure) return Result.Failure<Guid, Error>(titleResult.Error);
        var trackTitle = titleResult.Value;

        if (await _trackRepository.DoesArtistHaveTrackWithTitleAsync(artist.Id, trackTitle))
        {
            return Result.Failure<Guid, Error>(Errors.Module.AlreadyExist("Track with this title already exists."));
        }
        
        // === Крок 4: Збереження файлу ===
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.AudioFile.FileName);
        // Створюємо відносний шлях, який будемо зберігати в БД
        var relativePath = Path.Combine("/tracks", fileName); // Шлях починається зі слеша
        var absolutePath = Path.Combine("wwwroot", "tracks", fileName);
        
        using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await request.AudioFile.CopyToAsync(stream, cancellationToken);
        }
        
        // Тут має бути ваша логіка збереження файлу
        
        //var trackCreateResult = Domain.Entites.Track.Create(artist.Id, trackTitle, request.Duration, request.Valume, null, relativePath);
        var trackCreateResult = _trackDirector.BuildTrack( // <--- ТУТ ВИКОРИСТОВУЄМО ДИРЕКТОРА
            trackTitle,                 // Передаємо Title
            request.Duration,           // Передаємо Duration
            artist.Id,                  // Передаємо ArtistId
            relativePath,               // Передаємо FilePath
            request.Valume,             // Передаємо Valume
            request.collaborationNote);
        
        if(trackCreateResult.IsFailure) 
            return Result.Failure<Guid, Error>(trackCreateResult.Error);

        
        var track = trackCreateResult.Value;
        
        await _trackRepository.AddAsync(track);
        await _dbContextAccess.SaveChangesAsync(cancellationToken);
        
        await _publishEndpoint.Publish(new TrackCreatedEvent
        {
            TrackId = track.Id.Value,
            Title = trackTitle.Value,
            ArtistId = artist.Id.Value,
        });

        return Result.Success<Guid, Error>(track.Id.Value);
    }
}