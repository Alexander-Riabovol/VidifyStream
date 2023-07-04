namespace Data.Dtos.User
{
    public record UserRegisterDTO
    {
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new DateTime();
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
