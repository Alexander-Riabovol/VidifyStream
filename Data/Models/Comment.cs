using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
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
        public Comment() { }
    }
}
