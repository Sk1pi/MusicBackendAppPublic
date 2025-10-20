using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites;

public class Track
{
    public TrackId Id { get; init; }
    public int Valume { get; set; }
    public Title Title { get; set; }
    public TimeSpan Duration { get; set; }
    
    public ArtistId ArtistId { get; set; }
    public Artist Author { get; private set; }
    public string? CollaborationNote { get; set; }
    public string FilePath { get; set; }
    
    public ICollection<User> FavoritedByUsers { get; private set; } = new List<User>(); 
    
    public Track()
    { }
    
    public static Result<Track, Error> Create(
        ArtistId artistId, 
        Title title, 
        TimeSpan duration, 
        int volume,
        string? collaborationNote, 
        string filePath)
    {
        if (artistId == default)
        {
            return Result.Failure<Track, Error>(Errors.General.ValueIsRequired("ArtistId"));
        }
        
        return Result.Success<Track, Error>(new Track 
        {
            Id = TrackId.New(), 
            Title = title,
            Duration = duration,
            Valume = volume,
            ArtistId = artistId,
            CollaborationNote = collaborationNote,
            FilePath = filePath
        });
    }
}