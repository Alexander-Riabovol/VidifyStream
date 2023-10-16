using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get.Replies
{
    /// <summary>
    /// Retrieves all replies to a specific <see cref="Comment"/>.
    /// </summary>
    public record GetRepliesQuery(int CommentId) : IRequest<ServiceResponse<IEnumerable<ReplyGetDTO>>>;
}
