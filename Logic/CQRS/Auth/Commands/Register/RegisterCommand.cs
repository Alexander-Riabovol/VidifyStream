using VidifyStream.Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <returns>A service response containing the registered user's ID on success, or an error response if registration fails.</returns>
    public record RegisterCommand(
        string Name,
        DateTime BirthDate,
        string Email,
        string Password) : IRequest<ServiceResponse<int>>;
}
