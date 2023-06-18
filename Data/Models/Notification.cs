namespace Data.Models
{
    public record Notification
    {
        public int Id { get; set; }
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
