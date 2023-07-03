using Data.Dtos;
using Data.Dtos.Comment;

namespace Logic.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<IEnumerable<ReplyGetDTO>>> GetReplies(int commentId);
        Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId);
        Task<ServiceResponse<CommentGetDTO>> GetComment(int commentId);
        Task<ServiceResponse> PostComment(CommentPostDTO commentPostDTO);
        Task<ServiceResponse> PostReply(ReplyPostDTO replyPostDTO);
        Task<ServiceResponse> Put(CommentPutDTO commentPutDTO);
    }
}
