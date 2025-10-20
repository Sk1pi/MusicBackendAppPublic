using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicBackendApp.Domain.Entites;
using MusicBackendApp.Domain.Entites.Id_s;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;

namespace MusicBackendApp.Infrastructure.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(user => user.Id)
            .HasConversion(
                userId => userId.Value,
                value => new TrackId(value)
            );

        builder.HasOne(t => t.Author) 
            .WithMany(a => a.AuthoredTracks) 
            .HasForeignKey(t => t.ArtistId);
        
        builder.HasMany(t => t.FavoritedByUsers) 
            .WithMany(u => u.FavoriteTracks);
        
        builder.OwnsOne(x => x.Title, navigationBuilder =>
        {
            navigationBuilder.Property(title => title.Value)
                .HasColumnName("Title")
                .HasMaxLength(Title.MaxLength)
                .IsRequired();
        });
        
        builder.Property(t => t.CollaborationNote)
            .HasColumnName("CollaborationNote")
            .HasMaxLength(200); 
    }
}