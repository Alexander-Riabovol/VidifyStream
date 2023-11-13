using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Put.Like
{
    /// <summary>
    /// Leaves or removes a like from the <see cref="Comment"/> depending on the state of the like.
    /// </summary>
    public record ToggleLikeCommand(int CommandId) : IRequest<ServiceResponse>;
}
