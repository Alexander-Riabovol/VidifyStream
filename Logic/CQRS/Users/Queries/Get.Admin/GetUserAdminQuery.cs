using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get.Admin
{
    /// <summary>
    /// Retrieves all info about <see cref="User"/> by their ID.
    /// </summary>
    public record GetUserAdminQuery(int UserId) : IRequest<ServiceResponse<UserAdminGetDTO>>;
}
