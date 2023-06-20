namespace Data.Dtos.Comment
{
    public record CommentReplyGetDTO
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public int? RepliedToId { get; set; }
        public int LikesCount { get; set; }
        public bool IsAuthorLiked { get; set; }
        public bool Edited { get; set; }
    }
}
