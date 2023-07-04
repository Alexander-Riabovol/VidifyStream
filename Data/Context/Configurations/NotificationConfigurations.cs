using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    internal class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.NotificationId);

            builder.Property(n => n.Message)
                   .IsRequired();
            
            builder.Property(n => n.IsRead)
                   .IsRequired();

            builder.Property(n => n.Date)
                   .IsRequired();

            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Comment)
                   .WithMany()
                   .HasForeignKey(n => n.CommentId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Video)
                   .WithMany()
                   .HasForeignKey(n => n.VideoId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
