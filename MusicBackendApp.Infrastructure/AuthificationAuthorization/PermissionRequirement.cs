using Microsoft.AspNetCore.Authorization;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;

namespace MusicBackendApp.Infrastructure.AuthificationAuthorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission[] permissions) 
    {
        Permissions = permissions;
    }
    
    public Permission[] Permissions { get; set; } = [];
}