﻿using Microsoft.AspNetCore.Http;

namespace VidifyStream.Data.Dtos.User
{
    public record UserAvatarPostDTO
    {
        public IFormFile File { get; set; } = null!;
    }
}
