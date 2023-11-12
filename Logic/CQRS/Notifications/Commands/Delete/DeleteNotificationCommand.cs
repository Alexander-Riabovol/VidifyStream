using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Delete
{
    /// <summary>
    /// Deletes a <see cref="Notification"/> by ID.
    /// </summary>
    /// <param name="NotificationId">The ID of the notification.</param>
    public record DeleteNotificationCommand(int NotificationId) : IRequest<ServiceResponse>;
}
