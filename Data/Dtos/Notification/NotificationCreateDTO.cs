using Data.Models;

namespace Data.Dtos.Notification
{
    public record NotificationCreateDTO
    {
        public string Message { get; set; } = null!;
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public int? CommentId { get; set; }
        public int? VideoId { get; set; }
    }
}
