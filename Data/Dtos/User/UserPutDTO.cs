namespace Data.Dtos.User
{
    public record UserPutDTO
    {
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Bio { get; set; }
    }
}
