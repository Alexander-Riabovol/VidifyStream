namespace Data.Dtos.User
{
    public record UserAdminDeleteDTO
    {
        public int UserId { get; set; }
        public string BanMessage { get; set; } = null!;
    }
}
