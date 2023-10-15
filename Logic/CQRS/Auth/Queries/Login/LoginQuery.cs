using VidifyStream.Data.Dtos;
using MediatR;
using VidifyStream.Data.Dtos.User;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Login
{
    /// <summary>
    /// Logs in a user with the provided login data.
    /// </summary>
    /// <returns>A service response indicating the result of the login operation.</returns>
    public record LoginQuery(UserLoginDTO User) : IRequest<ServiceResponse>;
}
