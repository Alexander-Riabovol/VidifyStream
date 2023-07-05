namespace Data.Models
{
    /// <summary>
    /// A piece of information about an important event.
    /// Intended to be sent via SignalR directly to a <see cref="Models.User"/>.
    /// </summary>
    public record Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsRead { get; set; } = false;
        public int UserId { get; set; }
        public User? User { get; set; }
        public NotificationType Type { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public int? VideoId { get; set; }
        public Video? Video { get; set; }
    }
}
