namespace Data.Models
{
    /// <summary>
    /// A comment beneath a <see cref="Data.Models.Video"/>!
    /// </summary>
    // Navigation Properties Relationships:
    // Comment -> User    | Many-to-One
    // Comment -> Video   | Many-to-One
    // Comment -> Comment | One-to-Many & Many-to-One (Replies & RepliesTo)
    // Comment -> User    | Many-to-Many (Likes)
    public record Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public User? User { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
        public int? RepliedToId { get; set; }
        public Comment? RepliedTo { get; set; }
        public List<Comment>? Replies { get; set; }
        public List<User>? Likes { get; set; }
        public bool IsAuthorLiked { get; set; }
        public bool Edited { get; set; }
        public Comment() { }
    }
}
