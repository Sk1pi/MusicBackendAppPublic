using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.RolePermission;
using MusicBackendApp.Infrastructure.DataBase;

namespace MusicBackendApp.Infrastructure.Configurations.RolePermission.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
{
    private readonly AuthorizationOptions _options;

    public RolePermissionConfiguration(AuthorizationOptions options)
    {
        _options = options;
    }

    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasKey(r => new { r.RoleId, r.PermissionId });

        builder.HasData(ParseRolePermissions()!);
    }

    private RolePermissionEntity[]? ParseRolePermissions()
    {
        return (RolePermissionEntity[]?)_options.RolePermissions
            .SelectMany(rp => rp.Permission
                .Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p)
                }))
            .ToArray();
    }
}