using CSharpFunctionalExtensions;
using MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Builders;

public class TrackBuilder : ITrackBuilder
{
    private Domain.Entites.Track _track; // Це буде об'єкт, який ми будуємо

    // ПОМИЛКА: Конструктор приймає існуючий Track
    // Правильно: Конструктор має ініціалізувати НОВИЙ Track (або його базові компоненти)
    public TrackBuilder() // <--- Виправлено: Конструктор без параметрів
    {
        // Ініціалізуємо новий трек з унікальним ID
        _track = new Domain.Entites.Track { Id = TrackId.New() }; 
        // Якщо у Track є приватний конструктор, як у User/Artist, то потрібно,
        // щоб він мав protected-конструктор для ORM або щоб Builder використовував фабричний метод Track.Create()
        // Або створити "чистий" конструктор для Builder-а
    }

    // ПОМИЛКА: Не приймає параметр і повертає Result
    public ITrackBuilder WithTitle(Title title) // <--- Виправлено: Приймає параметр і повертає this
    {
        _track.Title = title;
        return this; // Для Fluent Interface
    }

    // ПОМИЛКА: Не приймає параметр і повертає Result
    public ITrackBuilder WithDuration(TimeSpan duration) // <--- Виправлено
    {
        // Логіка перевірки тривалості (більше 10 хв) тут недоречна. Це бізнес-правило,
        // яке має бути в валідаторі команди або в методі Track.Create()
        // Будівельник має просто "будувати", а не валідувати БІЗНЕС-ПРАВИЛА.
        _track.Duration = duration;
        return this;
    }

    // ... Реалізуй інші методи With... аналогічно, щоб вони приймали параметри і повертали this
    public ITrackBuilder WithArtist(ArtistId artistId)
    {
        _track.ArtistId = artistId; return this;
    }

    public ITrackBuilder WithFilePath(string filePath)
    {
        _track.FilePath = filePath; return this;
    }

    public ITrackBuilder WithVolume(int volume)
    {
        _track.Valume = volume; return this;
    }

    public ITrackBuilder WithCollaborationNote(string? note)
    {
        _track.CollaborationNote = note; return this;
    }

    // Метод Build()
    public Result<Domain.Entites.Track, Error> Build()
    {
        // Тут ти можеш додати ФІНАЛЬНІ перевірки (інваріанти), що об'єкт готовий.
        // Наприклад, перевірка, що Title і Duration не null.
        if (_track.Title == null || _track.Duration == TimeSpan.Zero || _track.ArtistId == null)
        {
            //throw new InvalidOperationException("Cannot build Track: missing essential components.");
            // Або, якщо ти хочеш використовувати Result тут:
            return Result.Failure<Domain.Entites.Track, Error>(Errors.General.ValueIsRequired("Track essential data"));
        }
        
        Domain.Entites.Track builtTrack = _track;
        // _track = new Domain.Entites.Track { Id = TrackId.New() }; // Очистити для наступної побудови (якщо builder багато разовий)
        return builtTrack;
        
    }
}
    /*public Result<Title, Error> WithTitle()
    {
        var titleResult = Title.Create(_track.Title.Value);
        if (titleResult.IsFailure) return Result.Failure<Title, Error>(titleResult.Error);
        var trackTitle = titleResult;
        
        return Result.Success<Title, Error>(trackTitle.Value);
    }

    public Result<TimeSpan, Error> WithDuration()
    {
        var durationResult = _track.Duration;
        if (durationResult.Equals(TimeSpan.FromMinutes(10)))
        {
            //має бути реалізація, якщо пісня більше 10 хв, то її ділять на частини
            return Result.Failure<TimeSpan, Error>(null);
        }
        
        return Result.Success<TimeSpan, Error>(durationResult);
    }*/
