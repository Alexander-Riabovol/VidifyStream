using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    // SavedPlaylists navigation property is configured in PlaylistConfigurations
    // ViewData navigation property is configured in ViewDataConfigurations
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Name)
                   .IsRequired();

            builder.Property(u => u.BirthDate)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired();

            builder.Property(u => u.Password)
                   .IsRequired();


            builder.HasMany(u => u.Videos)
                   .WithOne(v => v.User)
                   .HasForeignKey(v => v.UserId)
                   .IsRequired();
            
            builder.HasMany(u => u.Comments)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.ProfilePictureUrls)
                .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            builder.Property(u => u.UsedIPs)
                .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
