using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Entites.RolePermission;
using MusicBackendApp.Domain.Shared;
using MusicBackendApp.Infrastructure.DataBase;

namespace MusicBackendApp.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContextAccess _contextAccess;

    public UserRepository(DbContextAccess contextAccess)
    {
        _contextAccess = contextAccess;
    }
    
    public async Task<Result<User, Error>> GetByIdAsync(UserId id)
    {
        var users = await _contextAccess.Users
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return users is not null
            ? Result.Success<User, Error>(users)
            : Result.Failure<User, Error>(Errors.General.NotFound());
    }

    public async Task<Result<User, Error>> FindByEmailAsync(Email email)
    {
        var user = await _contextAccess.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value);
        
        if (user is null)
        {
            return Result.Failure<User, Error>(Errors.General.NotFound());
        }
        
        return Result.Success<User, Error>(user);
    }

    public async Task<Result<IQueryable<User>, Error>> GetByName(UserName name)
    {
        var query = _contextAccess.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name.Value))
        {
            query = query.Where(x => x.Name!.Value.Contains(name.Value));
        }
        
        return Result.Success<IQueryable<User>, Error>(query);
    }

    public async Task AddAsync(User user)
    {
        await _contextAccess.Users.AddAsync(user);
    }

    public void DeleteAsync(User user)
    {
       _contextAccess.Users.Remove(user);
    }

    public async Task<User?> GetUserWithFavoritesAsync(UserId userId)
    {
        return await _contextAccess.Users
            .Include(u => u.FavoriteTracks)
            .ThenInclude(t => t.Author) 
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _contextAccess.SaveChangesAsync();
    }

    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        var roles = await _contextAccess.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(u => u.Permissions)
            .Where(u => u.Id.Value == userId)
            .Select(u => u.Roles)
            .ToListAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }

    public async Task<int> UpdateUserAsync(User user, User newName)
    {
       return await _contextAccess.Users
            .Where(p => p.Id == user.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Name, newName.Name));
    }
}