using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.GetAll.Admin
{
    /// <summary>
    /// Retrieves all <see cref="Notification"/>s for a specific <see cref="User"/> the way they are stored in the database.
    /// </summary>
    /// <param name="UserId">The ID of the user.</param>
    public record GetAllNotificationsAdminQuery(int UserId) : IRequest<ServiceResponse<IEnumerable<NotificationAdminGetDTO>>>;
}
