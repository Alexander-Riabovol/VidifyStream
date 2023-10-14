using VidifyStream.Data.Models;
using Microsoft.AspNetCore.Http;

namespace VidifyStream.Data.Dtos.Video
{
    public record VideoPutDTO
    {
        public int VideoId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
}
