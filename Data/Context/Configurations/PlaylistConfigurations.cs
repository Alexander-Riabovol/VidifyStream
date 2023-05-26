using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    public class PlaylistConfigurations : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(p => p.PlaylistId);

            builder.Property(p => p.Name)
                   .IsRequired();

            // One-to-many relationships between User and Playlist.
            // The context is that a Playlist can be created by only one User,
            // while a User can create multiply Playlists.
            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);

            // Many-to-many relationships between User and Playlist.
            // They are chosen because Users can save multiply playlists,
            // as well as Playlists can be saved by multiply users.
            builder.HasMany(p => p.SavedByUsers)
                   .WithMany(u => u.SavedPlaylists)
                   .UsingEntity(
                       "UsersToSavedPlaylists",
                       // By default the Delete Behavior is Cascade. SqlServer says that
                       // FOREIGN KEYS may cause cycles or multiple cascade paths.
                       // So we need to redefine the Delete Behavior.
                       j =>
                       {
                            j.HasOne(typeof(Playlist)).WithMany().HasForeignKey("SavedPlaylistsPlaylistId").OnDelete(DeleteBehavior.NoAction);
                            j.HasOne(typeof(User)).WithMany().HasForeignKey("SavedByUsersUserId").OnDelete(DeleteBehavior.NoAction);
                       });

            // Many-to-many relationships between Playlist and Video.
            // They are chosen because one Playlist can contain multiply Videos,
            // and one Video can be shared into multiply Playlists.
            builder.HasMany(p => p.Videos)
                   .WithMany()
                   .UsingEntity("PlaylistsToVideos");
        }
    }
}
