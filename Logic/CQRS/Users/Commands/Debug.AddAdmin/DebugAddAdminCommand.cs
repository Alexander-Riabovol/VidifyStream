using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Debug.AddAdmin
{
    public record DebugAddAdminCommand : IRequest<ServiceResponse<User>>;
}
