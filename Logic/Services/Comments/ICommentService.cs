using Data.Models;
using Data.Dtos;
using Data.Dtos.Comment;

namespace Logic.Services.Comments
{
    /// <summary>
    /// Represents the interface for <see cref="Comment"/>-related services.
    /// It defines a contract for classes that provide operations
    /// such as retrieving comments, posting comments and replies,
    /// updating comments, and deleting comments.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Retrieves a <see cref="Comment"/> by its ID.
        /// </summary>
        Task<ServiceResponse<CommentGetDTO>> GetComment(int commentId);
        /// <summary>
        /// Retrieves all <see cref="Comment"/>s associated with a specific <see cref="Video"/>.
        /// </summary>
        Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId);
        /// <summary>
        /// Retrieves all replies to a specific <see cref="Comment"/>.
        /// </summary>
        Task<ServiceResponse<IEnumerable<ReplyGetDTO>>> GetReplies(int commentId);
        /// <summary>
        /// Adds a new <see cref="Comment"/> into the database.
        /// </summary>
        Task<ServiceResponse<int>> PostComment(CommentPostDTO commentPostDTO);
        /// <summary>
        /// Adds a new reply to a <see cref="Comment"/> into the database.
        /// </summary>
        Task<ServiceResponse<int>> PostReply(ReplyPostDTO replyPostDTO);
        /// <summary>
        /// Updates an existing <see cref="Comment"/>.
        /// </summary>
        Task<ServiceResponse> Put(CommentPutDTO commentPutDTO);
        /// <summary>
        /// Leaves or removes a like from the <see cref="Comment"/> depending on the state of the like.
        /// </summary>
        Task<ServiceResponse> ToggleLike(int commentId);
        /// <summary>
        /// Deletes a <see cref="Comment"/> by its ID if it's the authenticated <see cref="User"/>'s comment.
        /// </summary>
        Task<ServiceResponse> Delete(int commentId);
        /// <summary>
        /// Deletes any <see cref="Comment"/> by its ID.
        /// </summary>
        Task<ServiceResponse> DeleteAdmin(int commentId);
    }
}
