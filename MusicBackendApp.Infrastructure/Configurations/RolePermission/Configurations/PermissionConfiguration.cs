using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.RolePermission;

namespace MusicBackendApp.Infrastructure.Configurations.RolePermission.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(x => x.Id);

        var permissions = Enum
            .GetValues<Permission>()
            .Select(x => new PermissionEntity
            {
                Id = (int)x,
                Name = x.ToString()
            });
        
        builder.HasData(permissions);
    }
}