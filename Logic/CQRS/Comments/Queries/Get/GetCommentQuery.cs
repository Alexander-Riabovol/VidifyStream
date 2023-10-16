using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get
{
    /// <summary>
    /// Retrieves a <see cref="Comment"/> by its ID.
    /// </summary>
    public record GetCommentQuery(int CommentId) : IRequest<ServiceResponse<CommentGetDTO>>;
}
