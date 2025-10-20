using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Entites.RolePermission;
using MusicBackendApp.Domain.Shared;
using Error = MusicBackendApp.Domain.Shared.Error;
using Result = CSharpFunctionalExtensions.Result;


namespace MusicBackendApp.Domain.Entites;

public class User 
{
    public UserId Id { get; init; }
    public UserName Name { get; init; }
    public Password Password { get; init; }
    public Email Email { get; init; }
    public decimal LikedTracks { get; init; }
    public int Subs { get; init; }
    
    public ICollection<RoleEntity> Roles { get; init; } = new List<RoleEntity>();

    public ICollection<Permission> Permissions { get; init; } = [];
    
    public ICollection<Track> FavoriteTracks { get; private set; } = new List<Track>(); 

    private User()
    { }
    
    public static Result<User, Error> Create(UserName name, Email email, string hashedPassword, decimal likedTracks = 0, int subs = 0)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return Result.Failure<User, Error>(Errors.General.ValueIsInvalid("Hashed password"));
        }
        
        return Result.Success<User, Error>(new User 
        {
            Id = UserId.New(), 
            Name = name,
            Password = new Password(hashedPassword),
            Email = email,
            LikedTracks = likedTracks,
            Subs = subs
        });
    }

    public Result AddFavoriteTrack(Track trackToAdd)
    {
        if (trackToAdd == null)
        {
            Result.Failure(Errors.General.ValueIsRequired("Track").ToString());
        }
        
        if (FavoriteTracks.Any(t => t.Id == trackToAdd.Id))
        {
            return Result.Failure(Errors.Users.TrackAlreadyInFavorites(trackToAdd.Title.Value).ToString()); 
        }
        
        ((List<Track>)FavoriteTracks).Add(trackToAdd); 
        
        return Result.Success();
    }
    
    public Result RemoveFavoriteTrack(TrackId trackId)
    {
        if (trackId == null)
        {
            return Result.Failure(Errors.General.ValueIsRequired("Track ID").ToString());
        }

        var trackToRemove = FavoriteTracks.FirstOrDefault(t => t.Id == trackId);

        if (trackToRemove == null)
        {
            return Result.Failure(Errors.Users.TrackNotInFavorites().ToString());
        }

        ((List<Track>)FavoriteTracks).Remove(trackToRemove);
        return Result.Success();
    }
}