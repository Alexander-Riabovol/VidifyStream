using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    // Video -> User: Many-to-One
    // Video -> Comment: One-to-Many
    public record Video
    {
        public int VideoId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string SourceUrl { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ViewData>? ViewData { get; set; }
        public Video() { }
    }
}
