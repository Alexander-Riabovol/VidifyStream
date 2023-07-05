﻿using Data.Models;
using Data.Dtos;
using Data.Dtos.Notification;

namespace Logic.Services.NotificationService
{
    /// <summary>
    /// Represents a notification service interface for managing notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves all <see cref="Notification"/>s for a calling <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="onlyUnread">A flag indicating whether to retrieve only unread notifications. Default is true.</param>
        Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> GetAll(int userId, bool onlyUnread = true);
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
        /// Creates a new <see cref="Notification"/> and sends it.
        /// </summary>
        Task<ServiceResponse> CreateAndSend(NotificationCreateDTO notificationCreateDto);
        /// <summary>
        /// Toggles the IsRead property of a <see cref="Notification"/> to true.
        /// </summary>
        /// <param name="notificationId">The ID of the notification.</param>
        /// <param name="callerId">The ID of the caller performing the operation. Set to -1 to bypass security checks.</param>
        Task<ServiceResponse> ToggleTrueIsRead(int notificationId, int callerId = -1);
        /// <summary>
        /// Deletes a <see cref="Notification"/> by ID.
        /// </summary>
        /// <param name="notificationId">The ID of the notification.</param>
        /// <param name="callerId">The ID of the caller performing the operation. Set to -1 to bypass security checks.</param>
        Task<ServiceResponse> Delete(int notificationId, int callerId = -1);
    }
}
