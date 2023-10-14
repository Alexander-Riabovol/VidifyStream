using VidifyStream.Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Logout
{
    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>A service response indicating the result of the logout operation.</returns>
    public record LogoutQuery() : IRequest<ServiceResponse>;
}
