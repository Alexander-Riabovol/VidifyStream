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
        /// <summary>
        /// Creates a new <see cref="Notification"/> and sends it. Supposed to be used only by the server.
        /// </summary>
        Task<ServiceResponse> CreateAndSend(Notification notification);
        /// <summary>
        /// Creates a new <see cref="Notification"/> and sends it.
        /// </summary>
        Task<ServiceResponse> CreateAndSendAdmin(NotificationAdminCreateDTO notificationAdminCreateDto);
        /// <summary>
        /// Toggles the IsRead property of a <see cref="Notification"/> to true.
        /// </summary>
        /// <param name="notificationId">The ID of the notification.</param>
        Task<ServiceResponse> ToggleTrueIsRead(int notificationId);
        /// <summary>
        /// Deletes a <see cref="Notification"/> by ID.
        /// </summary>
        /// <param name="notificationId">The ID of the notification.</param>
        Task<ServiceResponse> Delete(int notificationId);
    }
}
