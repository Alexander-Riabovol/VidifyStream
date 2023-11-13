using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.GetAll.My
{
    /// <summary>
    /// Retrieves all <see cref="Notification"/>s for a calling <see cref="User"/>.
    /// </summary>
    /// <param name="UnreadOnly">A flag indicating whether to retrieve unread notifications only. Default is true.</param>
    public record GetAllMyNotificationsQuery(bool UnreadOnly = true) :
        IRequest<ServiceResponse<IEnumerable<NotificationGetDTO>>>;
}
