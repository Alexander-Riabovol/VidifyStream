namespace Data.Models
{
    /// <summary>
    /// This model is designed to collect information about
    /// viewers of a <see cref="Data.Models.Video"/> in order to provide analysis
    /// of target audience and improve the overall customer experience.
    /// </summary>
    // Navigation Properties Relationships:
    // ViewData -> User  | Many-to-One
    // ViewData -> Video | Many-to-One
    public record ViewData
    {
        //PK 1
        public int UserId { get; set; }
        public User? User { get; set; }
        //PK 2
        public int VideoId { get; set; }
        public Video? Video { get; set; }

        public int TimesViewed { get; set; }
        public int AverageViewTime { get; set; }
        public int PausedTime { get; set; }
        public ViewData() { }
    }
}
