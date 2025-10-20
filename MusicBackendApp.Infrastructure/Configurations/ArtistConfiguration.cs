using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(x => x.Id); 
        
        builder.Property(artist => artist.Id)
            .HasConversion(
                artistId => artistId.Value, 
                value => new ArtistId(value) 
            );
        
        builder.Property(artist => artist.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value)
            );
        
        builder.HasMany(a => a.AuthoredTracks) 
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.ArtistId); 
        
        builder.OwnsOne(x => x.ArtistName, navigationBuilder =>
        {
            navigationBuilder.Property(name => name.Value)
                .HasColumnName("ArtistName") 
                .HasMaxLength(ArtistName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, navigationBuilder =>
        {
            navigationBuilder.Property(email => email.Value)
                .HasColumnName("Email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Password, navigationBuilder =>
        {
            navigationBuilder.Property(password => password.Value)
                .HasColumnName("PasswordHash")
                .HasMaxLength(Password.MaxLength)
                .IsRequired();
        });
    }
}
