using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(user => user.Id)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value)
            );
        
        builder.HasMany(u => u.FavoriteTracks) 
            .WithMany(t => t.FavoritedByUsers);
        
        builder.OwnsOne(x => x.Name, navigationBuilder =>
        {
            navigationBuilder.Property(name => name.Value)
                .HasColumnName("UserName")
                .HasMaxLength(UserName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Password, navigationBuilder =>
        {
            navigationBuilder.Property(password => password.Value)
                .HasColumnName("PasswordHash")
                .HasMaxLength(Password.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, navigationBuilder =>
        {
            navigationBuilder.Property(email => email.Value)
                .HasColumnName("Email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();
        });
    }
}
