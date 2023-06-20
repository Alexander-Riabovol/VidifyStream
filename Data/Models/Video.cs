namespace Data.Models
{
    /// <summary>
    /// Represents a set of information about a piece of a film.
    /// The raw data itself is contained at <see href="SourceUrl"/>.
    /// </summary>
    // Navigation Properties Relationships:
    // Video -> User     | Many-to-One
    // Video -> Comment  | One-to-Many
    // Video -> ViewData | One-to-Many
    public record Video
    {
        public int VideoId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Category Category { get; set; }
        public string SourceUrl { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ViewData>? ViewData { get; set; }
        public Video() { }
    }
}
