namespace Data.Models
{
    /// <summary>
    /// A playlist represents an unordered collection of <see cref="Data.Models.Video"/>s.
    /// Only one <see cref="Data.Models.User"/> can modify a playlist.
    /// </summary>
    // Navigation Properties Relationships:
    // Playlist -> Video | Many-to-Many
    // Playlist -> User  | Many-to-One
    // Playlist -> User  | Many-to-Many (SavedByUsers)
    public record Playlist
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Video>? Videos { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public bool IsPublic { get; set; }
        public List<User>? SavedByUsers { get; set; }
        public Playlist() { }
    }
}
