using VidifyStream.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VidifyStream.Data.Context.Configurations
{
    /// <summary>
    /// Configures the <see cref="Notification"/> database model for code-first
    /// migrations and its relationships with other models.
    /// </summary>
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

            // One-to-Many relationships between User and Notification.
            // A Notification can only be addressed to one User
            // However, a User can recieve many Notifications
            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationships between User and Notification.
            // A Comment can trigger an unknown ammount of Notifications,
            // But a Notification may be tied to only one or zero Comments.
            builder.HasOne(n => n.Comment)
                   .WithMany()
                   .HasForeignKey(n => n.CommentId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            // One-to-Many relationships between User and Notification.
            // The logic is the same as Notifications-Comment relationships
            builder.HasOne(n => n.Video)
                   .WithMany()
                   .HasForeignKey(n => n.VideoId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
