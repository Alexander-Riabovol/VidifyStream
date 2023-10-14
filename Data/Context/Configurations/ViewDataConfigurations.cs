using VidifyStream.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VidifyStream.Data.Context.Configurations
{
    /// <summary>
    /// Configures the <see cref="ViewData"/> database model for code-first
    /// migrations and its relationships with other models.
    /// </summary>
    internal class ViewDataConfigurations : IEntityTypeConfiguration<ViewData>
    {
        public void Configure(EntityTypeBuilder<ViewData> builder)
        {
            // The ViewData entity represents Many-to-Many relationships between
            // User and Video, so the key is a unique combination of UserId and VideoId.
            builder.HasKey(vd => new { vd.UserId, vd.VideoId });

            builder.HasOne(vd => vd.User)
                   .WithMany(u => u.ViewData)
                   .HasForeignKey(vd => vd.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vd => vd.Video)
                   .WithMany(v => v.ViewData)
                   .HasForeignKey(vd => vd.VideoId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
