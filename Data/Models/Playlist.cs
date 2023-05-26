using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
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
