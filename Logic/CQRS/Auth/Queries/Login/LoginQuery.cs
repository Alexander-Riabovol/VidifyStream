using Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<ServiceResponse>;
}
