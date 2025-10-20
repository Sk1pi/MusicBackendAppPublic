using MusicBackendApp.Domain.Entites.Enums.UserSub;

namespace MusicBackendApp.Domain.Entites.RolePermission;

public class RoleEntity
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public SubscriptionType subscriptionType { get; set; }
    
    public ICollection<PermissionEntity> Permissions { get; set; } = new List<PermissionEntity>();
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
}