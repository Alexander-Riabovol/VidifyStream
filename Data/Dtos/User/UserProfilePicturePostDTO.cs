using Microsoft.AspNetCore.Http;

namespace VidifyStream.Data.Dtos.User
{
    public record UserProfilePicturePostDTO
    {
        public IFormFile File { get; set; } = null!;
    }
}
