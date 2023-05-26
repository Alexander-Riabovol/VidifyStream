using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    public class VideoConfigurations : IEntityTypeConfiguration<Video>
    {
        // User navigation property is configured in UserConfigurations
        // ViewData navigation property is configured in ViewDataConfigurations
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasKey(v => v.VideoId);

            builder.Property(v => v.Title)
                   .IsRequired();

            builder.Property(v => v.Description)
                   .IsRequired();

            builder.Property(v => v.SourceUrl)
                   .IsRequired();

            builder.Property(v => v.ThumbnailUrl)
                   .IsRequired();

            builder.HasMany(v => v.Comments)
                   .WithOne(c => c.Video)
                   .HasForeignKey(c => c.VideoId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
