namespace VidifyStream.Data.Dtos.Comment
{
    public record CommentPostDTO
    {
        public int VideoId { get; set; }
        public string Text { get; set; } = null!;
    }
}
