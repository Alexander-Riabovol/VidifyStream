using Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    public record RegisterCommand(
        string Name,
        DateTime BirthDate,
        string Email,
        string Password) : IRequest<ServiceResponse<int>>;
}
