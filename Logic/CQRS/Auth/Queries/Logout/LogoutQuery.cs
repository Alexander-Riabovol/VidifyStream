using VidifyStream.Data.Dtos;
using MediatR;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Logout
{
    public record LogoutQuery() : IRequest<ServiceResponse>;
}
