namespace MusicBackendApp.Application.Common.Interfaces.Permission;

public interface IPermissionService
{
    Task<HashSet<Domain.Entites.Enums.RolePermission.Permission>> GetPermissionAsync(Guid userId);
}