using Data.Dtos;
using Data.Dtos.Notification;

namespace Logic.Services.NotificationService
{
    public interface INotificationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerId">If set to -1, there will be no security check.</param>
        /// <returns></returns>
        Task<ServiceResponse> ToggleTrueIsRead(int notificationId, int callerId = -1);
        Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> GetAll(int userId, bool onlyUnread = true);
        Task<ServiceResponse<NotificationAdminGetDTO>> GetAdmin(int notificationId);
        Task<ServiceResponse<IEnumerable<NotificationAdminGetDTO>>> GetAllAdmin(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerId">If set to -1, there will be no security check.</param>
        /// <returns></returns>
        Task<ServiceResponse> Delete(int notificationId, int callerId = -1);
        Task<ServiceResponse> CreateAndSend(NotificationCreateDTO notificationDto);
    }
}
