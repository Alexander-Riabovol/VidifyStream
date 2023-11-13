using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Push.Admin
{
    /// <summary>
    /// Creates a new <see cref="Notification"/> and sends it.
    /// </summary>
    public record PushNotificationAdminCommand(NotificationAdminCreateDTO NotificationDto)
        : IRequest<ServiceResponse>;
}
