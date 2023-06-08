using Data.Dtos;
using Data.Dtos.Notification;

namespace Logic.Services.NotificationService
{
    public interface INotificationService
    {
        Task<ServiceResponse> ToggleTrueIsRead(int notificationId);
        Task<ServiceResponse<IEnumerable<NotificationGetDTO>?>> GetAll(int userId, bool onlyUnread = true);
    }
}
