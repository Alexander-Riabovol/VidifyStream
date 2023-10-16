using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Delete.Me
{
    /// <summary>
    /// Deletes the current <see cref="User"/>.
    /// </summary>
    public record DeleteUserMeCommand() : IRequest<ServiceResponse>;
}
