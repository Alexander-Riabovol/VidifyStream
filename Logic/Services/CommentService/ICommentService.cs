using Data.Dtos;
using Data.Dtos.Comment;

namespace Logic.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<IEnumerable<CommentReplyGetDTO>>> GetReplies(int commentId);
        Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId);
        Task<ServiceResponse<CommentGetDTO>> GetComment(int commentId);
        Task<ServiceResponse> PostComment(int videoId, CommentPostDTO commentDto);
        Task<ServiceResponse> PostReply(int repliedToId, CommentPostDTO commentDto);
    }
}
