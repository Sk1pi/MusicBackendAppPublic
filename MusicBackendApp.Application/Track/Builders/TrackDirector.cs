using CSharpFunctionalExtensions;
using MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Builders;

public class TrackDirector
{
    private readonly ITrackBuilder _trackBuilder;

    public TrackDirector(ITrackBuilder trackBuilder)
    {
        _trackBuilder = trackBuilder;
    }

    // Цей єдиний метод приймає ВСІ можливі дані з команди
    public Result<Domain.Entites.Track, Error> BuildTrack(
        Title title,
        TimeSpan duration,
        ArtistId artistId,
        string filePath,
        int volume,
        string? collaborationNote) // collaborationNote тепер опціональний
    {
        _trackBuilder
            .WithTitle(title)
            .WithDuration(duration)
            .WithArtist(artistId)
            .WithFilePath(filePath)
            .WithVolume(volume);
        
        if (!string.IsNullOrWhiteSpace(collaborationNote))
        {
            _trackBuilder.WithCollaborationNote(collaborationNote);
        }
        
        var buildResult = _trackBuilder.Build();
        
        if (buildResult.IsFailure)
        {
            throw new InvalidOperationException(buildResult.Error.Message); // Приклад обробки
        }

        return buildResult.Value;
    }
}