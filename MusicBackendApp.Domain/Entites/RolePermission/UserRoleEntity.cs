namespace MusicBackendApp.Domain.Entites.RolePermission;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }

    public User User { get; init; } = null!;
    public RoleEntity Role { get; init; } = null!;
}