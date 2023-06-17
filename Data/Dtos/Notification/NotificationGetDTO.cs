namespace Data.Dtos.Notification
{
    public record NotificationGetDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string ActionLink { get; set; } = null!;
        public string UserIconLink { get; set; } = null!;
        public string VideoThumbnailLink { get; set; } = null!;
    }
}
