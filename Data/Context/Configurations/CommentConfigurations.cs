using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    /// <summary>
    /// Configures the <see cref="Comment"/> database model for code-first
    /// migrations and its relationships with other models.
    /// </summary>
    internal class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        // User  navigation property is configured in UserConfigurations
        // Video navigation property is configured in VideoConfigurations
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Ignore soft deleted entries
            builder.HasQueryFilter(x => x.DeletedAt == null);

            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.Text)
                   .IsRequired();

            // One-to-Many relationships between Comment and Comments.
            // The idea behind this is that there may be multiply replies to a comment,
            // But the replies can be only replied to a single comment.
            builder.HasMany(c => c.Replies)
                   .WithOne(c => c.RepliedTo)
                   .HasForeignKey(c => c.RepliedToId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            // Many-to-Many relationships between User and Comment.
            // Any User can like any comment, including their own. However, note that the like from the creator of 
            // a video must be displaied separetely, so we use the IsAuthorLiked property for that.
            builder.HasMany(c => c.Likes)
                   .WithMany()
                   .UsingEntity("Likes");
        }
    }
}
