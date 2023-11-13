using VidifyStream.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VidifyStream.Data.Context.Configurations
{
    /// <summary>
    /// Configures the <see cref="User"/> database model for code-first
    /// migrations and its relationships with other models.
    /// </summary>
    internal class UserConfigurations : IEntityTypeConfiguration<User>
    {
        // ViewData navigation property is configured in ViewDataConfigurations
        // Notifications navigation property is configured in NotificationConfigurations
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Ignore soft deleted entries
            builder.HasQueryFilter(x => x.DeletedAt == null);

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Name)
                   .IsRequired();

            builder.Property(u => u.BirthDate)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired();

            builder.Property(u => u.Password)
                   .IsRequired();


            // One-to-Many relationships between User and Video.
            // Represents videos published by a user.
            builder.HasMany(u => u.Videos)
                   .WithOne(v => v.User)
                   .HasForeignKey(v => v.UserId)
                   .IsRequired();

            // One-to-Many relationships between User and Comment.
            // A comment can be created only by a one user.
            builder.HasMany(u => u.Comments)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // To contain a List<string> variable in one table cell we use Join and Split methods.
            builder.Property(u => u.ProfilePictureUrls)
                .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
