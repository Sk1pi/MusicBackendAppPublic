using CSharpFunctionalExtensions;
using MusicBackendApp.Application.Common.Interfaces.Builders.Tracks;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Builders;

public class TrackBuilder : ITrackBuilder
{
    private Domain.Entites.Track _track; 
    
    public TrackBuilder() 
    {
        _track = new Domain.Entites.Track { Id = TrackId.New() }; 
    }
    
    public ITrackBuilder WithTitle(Title title) 
    {
        _track.Title = title;
        return this; 
    }
    
    public ITrackBuilder WithDuration(TimeSpan duration)
    {
        _track.Duration = duration;
        return this;
    }
    
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
    
    public Result<Domain.Entites.Track, Error> Build()
    {
        if (_track.Title == null || _track.Duration == TimeSpan.Zero || _track.ArtistId == null)
        {
            return Result.Failure<Domain.Entites.Track, Error>(Errors.General.ValueIsRequired("Track essential data"));
        }
        
        Domain.Entites.Track builtTrack = _track;
        return builtTrack;
        
    }
}

