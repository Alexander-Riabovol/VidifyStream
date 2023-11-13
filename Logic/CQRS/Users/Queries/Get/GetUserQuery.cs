using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get
{
    /// <summary>
    /// Retrieves a <see cref="User"/> by their ID.
    /// </summary>
    public record GetUserQuery(int UserId) : IRequest<ServiceResponse<UserGetDTO>>;
}
