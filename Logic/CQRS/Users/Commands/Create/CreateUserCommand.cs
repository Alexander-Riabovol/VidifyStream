using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Create
{
    /// <summary>
    /// Creates a new user. Shall not be directly accessible via api.
    /// </summary>
    public record CreateUserCommand(User User) : IRequest<ServiceResponse<int>>;
}
