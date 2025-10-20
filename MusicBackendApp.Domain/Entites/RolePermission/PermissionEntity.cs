namespace MusicBackendApp.Domain.Entites.RolePermission;

public class PermissionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RoleEntity> Roles { get; set; } = [];
}