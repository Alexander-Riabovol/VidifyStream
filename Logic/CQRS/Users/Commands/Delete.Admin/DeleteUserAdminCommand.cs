using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Delete.Admin
{
    /// <summary>
    /// Deletes a <see cref="User"/> by it's ID.
    /// </summary>
    public record DeleteUserAdminCommand(UserAdminDeleteDTO UserDto) : IRequest<ServiceResponse>;
}
