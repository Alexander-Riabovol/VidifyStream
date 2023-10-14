namespace VidifyStream.Data.Dtos.Notification
{
    public record NotificationAdminCreateDTO
    {
        public string Message { get; set; } = null!;
        public int UserId { get; set; }
    }
}
