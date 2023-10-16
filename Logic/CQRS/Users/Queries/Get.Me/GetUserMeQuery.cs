using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get.Me
{
    /// <summary>
    /// Retrieves the current <see cref="User"/>.
    /// </summary>
    public record GetUserMeQuery() : IRequest<ServiceResponse<UserGetMeDTO>>;
}
