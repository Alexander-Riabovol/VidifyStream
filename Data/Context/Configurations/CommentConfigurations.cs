using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configurations
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        // User navigation property is configured in UserConfigurations
        // Video navigation property is configured in VideoConfigurations
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.Text)
                   .IsRequired();

            builder.HasMany(c => c.Replies)
                   .WithOne(c => c.RepliedTo)
                   .HasForeignKey(c => c.RepliedToId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Likes)
                   .WithMany()
                   .UsingEntity("Likes");
        }
    }
}
