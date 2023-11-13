namespace VidifyStream.Data.Dtos.Comment
{
    public record CommentPutDTO
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = null!;
    }
}
