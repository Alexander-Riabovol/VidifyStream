namespace Data.Dtos.Notification
{
    public record NotificationGetDTO
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string ActionLink { get; set; } = null!;
        public string UserProfilePictureUrl { get; set; } = null!;
        public string VideoThumbnailUrl { get; set; } = null!;
    }
}
