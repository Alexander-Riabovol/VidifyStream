using Data.Context;
using Data.Dtos;
using Data.Dtos.Notification;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _dataContext;

        public NotificationService(DataContext dataContext) 
        { 
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<IEnumerable<NotificationGetDTO>?>> GetAll(int userId, bool onlyUnread)
        {
            var user = await _dataContext.Users
                                         .Include(u => u.Notifications)
                                         .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) 
            {
                return new ServiceResponse<IEnumerable<NotificationGetDTO>?>(404, "User not found");
            }

            if(onlyUnread)
            {
                user.Notifications = user.Notifications?.Where(n => !n.IsRead).ToList();
            }

            var notificationsDtos = user.Notifications?.Select(n => 
            // move mapping into a class
            new NotificationGetDTO()
            {
                Id = n.Id,
                Message = n.Message,
                Date = n.Date,
                IsRead = n.IsRead,
                // rest of the mapping with includes according to the message type here
            });

            return ServiceResponse<IEnumerable<NotificationGetDTO>?>.OK(notificationsDtos);
        }

        public Task<ServiceResponse> ToggleTrueIsRead(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
