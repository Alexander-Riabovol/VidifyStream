using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Push
{
    /// <summary>
    /// Creates a new notification and sends it. Shall not be accessible via api.
    /// </summary>
    public record PushNotificationCommand(Notification Notification) : IRequest<ServiceResponse>;
}