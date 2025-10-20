namespace MusicBackendApp.Domain.Entites.RolePermission;

public class RolePermissionEntity
{
    public int RoleId { get; init; }
    public int PermissionId { get; init; }

    public RoleEntity Role { get; init; } = null!;
    public PermissionEntity Permission { get; init; } = null!;
}