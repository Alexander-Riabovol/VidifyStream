using VidifyStream.Data.Models;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;

namespace VidifyStream.Logic.Services.Notifications
{
    /// <summary>
    /// Represents a notification service interface for managing notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves all <see cref="Notification"/>s for a calling <see cref="User"/>.
        /// </summary>
        /// <param name="onlyUnread">A flag indicating whether to retrieve only unread notifications. Default is true.</param>
        Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> GetAllMy(bool onlyUnread = true);
        /// <summary>
        /// Retrieves a <see cref="Notification"/> by its ID the way they are stored in the database.
        /// </summary>
        /// <param name="notificationId">The ID of the notification.</param>
        Task<ServiceResponse<NotificationAdminGetDTO>> GetAdmin(int notificationId);
        /// <summary>
        /// Retrieves all <see cref="Notification"/>s for a specific <see cref="User"/> the way they are stored in the database.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        Task<ServiceResponse<IEnumerable<NotificationAdminGetDTO>>> GetAllAdmin(int userId);
    }
}
