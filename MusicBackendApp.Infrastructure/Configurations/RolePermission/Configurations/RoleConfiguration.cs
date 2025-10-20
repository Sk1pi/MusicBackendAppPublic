using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.RolePermission;

namespace MusicBackendApp.Infrastructure.Configurations.RolePermission.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(r => r.Permissions)
            .WithMany(x => x.Roles)
            .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(e => e.PermissionId),
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(e => e.RoleId));
        
        var role = Enum
            .GetValues<Role>()
            .Select(x => new RoleEntity
            {
                Id = (int)x,
                Name = x.ToString()
            });
        
        builder.HasData(role);
    }
}