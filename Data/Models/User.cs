namespace Data.Models
{
    /// <summary>
    /// User data. Be careful to avoid leaking personal info.
    /// </summary>
    // Navigation Properties Relationships:
    // User -> Video    | One-to-Many
    // User -> Comments | One-to-Many
    // User -> Playlist | Many-to-Many
    // User -> ViewData | One-to-Many
    public record User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new DateTime();
        public string? Bio { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<string>? ProfilePictureUrls { get; set; }
        public List<string>? UsedIPs { get; set; }
        public Status Status { get; set; }
        public List<Video>? Videos { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ViewData>? ViewData { get; set; }
        public List<Notification>? Notifications { get; set; }
        public string? BanMessage { get; set; }
        public User() { }
    }
}
