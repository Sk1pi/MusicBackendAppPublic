using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;

public interface ITrackBuilder
{
    // ПОМИЛКА: Методи With... не приймають параметрів і повертають Result<T, Error>
    // Правильно: Вони повинні ПРИЙМАТИ параметри (дані) і повертати ІНТЕРФЕЙС БІЛДЕРА (this) для ланцюговості
    ITrackBuilder WithTitle(Title title); // Повинно приймати Title як параметр
    ITrackBuilder WithDuration(TimeSpan duration);
    ITrackBuilder WithArtist(ArtistId artistId); // Повинно приймати ArtistId
    ITrackBuilder WithFilePath(string filePath);
    ITrackBuilder WithVolume(int volume);
    ITrackBuilder WithCollaborationNote(string? note); // note не був вказаний в попередньому коді
    Result<Domain.Entites.Track, Error> Build();  
}