using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using Error = MusicBackendApp.Domain.Shared.Error;

namespace MusicBackendApp.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<Domain.Entites.User, Domain.Shared.Error>> GetByIdAsync(UserId id);
    
    Task<Result<Domain.Entites.User, Error>> FindByEmailAsync(Email email);
    Task<Result<IQueryable<Domain.Entites.User>, Error>> GetByName(UserName name); 
    Task AddAsync(Domain.Entites.User user);
    void DeleteAsync(Domain.Entites.User user);
    Task<Domain.Entites.User?> GetUserWithFavoritesAsync(UserId userId);
    
    Task UpdateUserAsync(Domain.Entites.User user); 
    
    Task<HashSet<Domain.Entites.Enums.RolePermission.Permission>> GetUserPermissions(Guid userId);
}