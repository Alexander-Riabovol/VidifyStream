using VidifyStream.Data.Dtos;
using MediatR;
using VidifyStream.Data.Dtos.User;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <returns>A service response containing the registered user's ID on success, or an error response if registration fails.</returns>
    public record RegisterCommand(UserRegisterDTO User) : IRequest<ServiceResponse<int>>;
}
