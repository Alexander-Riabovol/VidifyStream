using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Models;

namespace VidifyStream.Data.Dtos.User
{
    public record UserGetMeDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new DateTime();
        public string? Bio { get; set; }
        public string Email { get; set; } = null!;
        public List<string> ProfilePictureUrls { get; set; } = new List<string>();
        public Status Status { get; set; }
        public List<UserGetVideoDTO>? Videos { get; set; }
    }
}
