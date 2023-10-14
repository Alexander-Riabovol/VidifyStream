using VidifyStream.Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Login
{
    /// <summary>
    /// Logs in a user with the provided login data.
    /// </summary>
    /// <returns>A service response indicating the result of the login operation.</returns>
    public record LoginQuery(
        string Email,
        string Password) : IRequest<ServiceResponse>;
}
