using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites;

public class Artist 
{
    public ArtistId Id { get; init; }
    public ArtistName ArtistName { get; init; }
    public Email Email { get; init; }
    public Password Password { get; init; }
    public int Subs { get; init; }
    
    public UserId UserId { get; init; }
    
    public ICollection<Track> AuthoredTracks { get; private set; } = new List<Track>();

    private Artist() { }
    
    public static Result<Artist, Error> Create(
        ArtistName artistName, 
        Email email, 
        Password password, 
        UserId userId, 
        int subs)
    {
        if (userId == default) 
        {
            return Result.Failure<Artist, Error>(Errors.General.ValueIsRequired("UserId"));
        }

        return Result.Success<Artist, Error>(new Artist
        {
            Id = ArtistId.New(),
            UserId = userId, 
            ArtistName = artistName,
            Email = email,
            Password = password,
            Subs = subs
        });
    }
}