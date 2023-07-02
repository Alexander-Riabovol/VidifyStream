﻿using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Data.Dtos.Video
{
    public record VideoPostDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Category Category { get; set; }
        public IFormFile VideoFile { get; set; } = null!;
        public IFormFile? Thumbnail { get; set; }
    }
}