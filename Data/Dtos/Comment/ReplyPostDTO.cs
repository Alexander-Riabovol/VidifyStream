namespace VidifyStream.Data.Dtos.Comment
{
    public record ReplyPostDTO
    {
        public int RepliedToId { get; set; }
        public string Text { get; set; } = null!;
    }
}