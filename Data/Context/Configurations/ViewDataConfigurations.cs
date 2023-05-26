using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    public class ViewDataConfigurations : IEntityTypeConfiguration<ViewData>
    {
        public void Configure(EntityTypeBuilder<ViewData> builder)
        {
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
