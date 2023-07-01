using Data.Models;

namespace Data.Dtos.Video
{
    public record VideoGetDTO
    {
        public int VideoId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Category Category { get; set; }
        public string SourceUrl { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserProfilePictureUrl { get; set; } = null!;

    }
}
