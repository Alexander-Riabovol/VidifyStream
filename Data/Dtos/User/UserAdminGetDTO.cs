﻿using VidifyStream.Data.Models;

namespace VidifyStream.Data.Dtos.User
{
    public record UserAdminGetDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new DateTime();
        public string? Bio { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<string> ProfilePictureUrls { get; set; } = new List<string>();
        public Status Status { get; set; }
        public List<int>? VideosIds { get; set; }
        public List<int>? CommentsIds { get; set; }
        public List<int>? NotificationsIds { get; set; }
        public string? BanMessage { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
