namespace Data.Models
{
    // This table is manifistation of Many-to-Many
    public record ViewData
    {
        //PK 1
        public int UserId { get; set; }
        //PK 2
        public int VideoId { get; set; }
        public User? User { get; set; }
        public Video? Video { get; set; }

        public int TimesViewed { get; set; }
        public int AverageViewTime { get; set; }
        public int PausedTime { get; set; }
        public ViewData() { }
    }
}
