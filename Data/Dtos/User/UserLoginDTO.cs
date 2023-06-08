namespace Data.Dtos.User
{
    public record UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserLoginDTO() {}
    }
}
