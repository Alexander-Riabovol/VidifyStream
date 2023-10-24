using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Delete
{
    /// <summary>
    /// In case the Admin parameter is false, deletes a <see cref="Comment"/> by its ID,
    /// if it's the authenticated <see cref="User"/>'s comment.
    /// Otherwise, deletes any <see cref="Comment"/>.
    /// </summary>
    public record DeleteCommentCommand(int CommentId, bool Admin) : IRequest<ServiceResponse>;
}
