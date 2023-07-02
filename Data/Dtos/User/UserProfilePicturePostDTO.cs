using Microsoft.AspNetCore.Http;

namespace Data.Dtos.User
{
    public record UserProfilePicturePostDTO
    {
        public IFormFile File { get; set; } = null!;
    }
}
