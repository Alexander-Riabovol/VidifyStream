using Data.Dtos;
using Data.Dtos.Comment;

namespace Logic.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetReplies(int commentId);
        Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId);
        Task<ServiceResponse<CommentGetDTO>> GetComment(int commentId);
    }
}
