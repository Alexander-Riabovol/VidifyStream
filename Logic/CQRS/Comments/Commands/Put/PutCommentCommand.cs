using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Put
{
    /// <summary>
    /// Updates an existing <see cref="Comment"/>.
    /// </summary>
    public record PutCommentCommand(CommentPutDTO CommentDto) : IRequest<ServiceResponse>;
}
