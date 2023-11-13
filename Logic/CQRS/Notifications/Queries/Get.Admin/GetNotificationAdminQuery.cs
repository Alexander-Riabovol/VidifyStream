using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.Get.Admin
{
    /// <summary>
    /// Retrieves a <see cref="Notification"/> by its ID the way they are stored in the database.
    /// </summary>
    /// <param name="NotificationId">The ID of the notification.</param>
    public record GetNotificationAdminQuery(int NotificationId) : IRequest<ServiceResponse<NotificationAdminGetDTO>>;
}
