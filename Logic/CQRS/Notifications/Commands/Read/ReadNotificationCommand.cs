using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Read
{
    /// <summary>
    /// Toggles the IsRead property of a <see cref="Notification"/> to true.
    /// </summary>
    /// <param name="NotificatrionId">The ID of the notification.</param>
    public record ReadNotificationCommand(int NotificationId) : IRequest<ServiceResponse>;
}
