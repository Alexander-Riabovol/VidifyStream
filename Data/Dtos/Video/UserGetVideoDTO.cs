using VidifyStream.Data.Models;

namespace VidifyStream.Data.Dtos.Video
{
    public record UserGetVideoDTO
    {
        public int VideoId { get; set; }
        public string Title { get; set; } = null!;
        public Category Category { get; set; }
        public string SourceUrl { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
    }
}
