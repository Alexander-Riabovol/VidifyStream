using Data.Persistence;

namespace Data.Models
{
    /// <summary>
    /// User data. Be careful to avoid leaking personal info.
    /// </summary>
    // Navigation Properties Relationships:
    // User -> Video         | One-to-Many
    // User -> Comments      | One-to-Many
    // User -> ViewData      | One-to-Many
    // User -> Notifications | One-to-Many
    public record User : ISoftDelete
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new DateTime();
        public string? Bio { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<string> ProfilePictureUrls { get; set; } = new List<string>();
        public Status Status { get; set; }
        public List<Video>? Videos { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ViewData>? ViewData { get; set; }
        public List<Notification>? Notifications { get; set; }
        public string? BanMessage { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public User() { }
    }
}
