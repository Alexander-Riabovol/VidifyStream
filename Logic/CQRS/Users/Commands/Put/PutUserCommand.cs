using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Put
{
    /// <summary>
    /// Updates <see cref="User"/>'s personal information.
    /// </summary>
    public record PutUserCommand(UserPutDTO UserDto) : IRequest<ServiceResponse>;
}
