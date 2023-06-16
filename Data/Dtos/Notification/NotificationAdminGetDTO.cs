using Data.Models;

namespace Data.Dtos.Notification
{
    public record NotificationAdminGetDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public int? CommentId { get; set; }
        public int? VideoId { get; set; }
    }
}
