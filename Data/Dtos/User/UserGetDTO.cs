using Data.Dtos.Video;

namespace Data.Dtos.User
{
    public record UserGetDTO
    {
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
        public string ProfilePictureUrl { get; set; } = null!;
        public List<UserGetVideoDTO>? Videos { get; set; }
    }
}
