using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post
{
    /// <summary>
    /// Adds a new <see cref="Comment"/> into the database.
    /// </summary>
    public record PostCommentCommand(CommentPostDTO CommentDto) : IRequest<ServiceResponse<int>>;
}
