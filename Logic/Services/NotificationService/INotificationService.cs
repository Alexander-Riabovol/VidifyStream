using Data.Dtos;
using Data.Dtos.Notification;

namespace Logic.Services.NotificationService
{
    public interface INotificationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">If set to -1, there will be no security check.</param>
        /// <returns></returns>
        Task<ServiceResponse> ToggleTrueIsRead(int notificationId, int userId = -1);
        Task<ServiceResponse<IEnumerable<NotificationGetDTO>?>> GetAll(int userId, bool onlyUnread = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">If set to -1, there will be no security check.</param>
        /// <returns></returns>
        Task<ServiceResponse> Delete(int notificationId, int userId = -1);
    }
}
