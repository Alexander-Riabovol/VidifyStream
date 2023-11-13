using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post.Reply
{
    /// <summary>
    /// Adds a new reply to a <see cref="Comment"/> into the database.
    /// </summary>
    public record PostReplyCommand(ReplyPostDTO ReplyDto) : IRequest<ServiceResponse<int>>;
}
