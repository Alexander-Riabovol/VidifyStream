using Logic.Hubs;
using Data.Context;
using Data.Dtos;
using Data.Dtos.Notification;
using Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;

namespace Logic.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _dataContext;
        private readonly IHubContext<NotificationsHub> _notificationsHub;
        private readonly IMapper _mapper;

        public NotificationService(DataContext dataContext,
                                   IHubContext<NotificationsHub> notificationsHub,
                                   IMapper mapper) 
        {
            _dataContext = dataContext;
            _notificationsHub = notificationsHub;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAndSend(NotificationCreateDTO notificationCreateDto)
        {
            // return 404 if user to whom addressed a notification doesn't exist
            var user = await _dataContext.Users.FindAsync(notificationCreateDto.UserId);
            if(user == null) { return new ServiceResponse(404, "User Not Found");}

            // add validation here

            var notification = _mapper.Map<Notification>(notificationCreateDto);
            /*var notification = new Notification()
            {
                Message = notificationCreateDto.Message,
                Date = DateTime.Now,
                IsRead = false,
                UserId = notificationCreateDto.UserId,
                Type = notificationCreateDto.Type,
                CommentId = notificationCreateDto.CommentId,
                VideoId = notificationCreateDto.VideoId,
            };*/

            await _dataContext.AddAsync(notification);
            await _dataContext.SaveChangesAsync();

            var notificationGetDto = _mapper.Map<NotificationGetDTO>(notification);
            /*var notificationGetDto = new NotificationGetDTO()
            {
                // Id is present because we saved the changes into the database beforehand
                Id = notification.Id,
                Message = notification.Message,
                Date = notification.Date,
                IsRead = notification.IsRead
            };*/
            // broadcast the new notification to a user group to whom it addressed
            await _notificationsHub.Clients.Group($"push-{notification.UserId}")
                .SendAsync("push-notifications", new List<NotificationGetDTO> { notificationGetDto });

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> Delete(int notificationId, int callerId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);
            // If the notification does not exists, return 404
            if (notification == null)
            {
                return new ServiceResponse(404, $"Notification with {notificationId} id does not exist.");
            }
            // If userIds do not match, return 403
            if (callerId != -1 && notification.UserId != callerId)
            {
                return new ServiceResponse(403, "Forbidden");
            }
            _dataContext.Remove(notification);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse<NotificationAdminGetDTO>> GetAdmin(int notificationId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);

            if(notification == null)
            {
                return new ServiceResponse<NotificationAdminGetDTO>(404, "Notification has not been found.");
            }

            var notificationDto = _mapper.Map<NotificationAdminGetDTO>(notification);
            /*var notificationDto = new NotificationAdminGetDTO()
            {
                Id = notification.Id,
                Message = notification.Message,
                Date = notification.Date,
                IsRead = notification.IsRead,
                UserId = notification.UserId,
                Type = notification.Type,
                CommentId = notification.CommentId,
                VideoId = notification.VideoId,
            };*/
            return ServiceResponse<NotificationAdminGetDTO>.OK(notificationDto);
        }

        public async Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> GetAll(int userId, bool onlyUnread)
        {
            var user = await _dataContext.Users
                                         .Include(u => u.Notifications)
                                         .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) 
            {
                return new ServiceResponse<IEnumerable<NotificationGetDTO>>(404, "User has not been found.");
            }

            if (onlyUnread)
            {
                user.Notifications = user.Notifications?.Where(n => !n.IsRead).ToList();
            }

            var notificationsDtos = user.Notifications?.Select(n => 
            _mapper.Map<NotificationGetDTO>(n)
            /*new NotificationGetDTO()
            {
                Id = n.Id,
                Message = n.Message,
                Date = n.Date,
                IsRead = n.IsRead,
                // rest of the mapping with includes according to the message type here
            }*/);

            return ServiceResponse<IEnumerable<NotificationGetDTO>>.OK(notificationsDtos);
        }

        public async Task<ServiceResponse<IEnumerable<NotificationAdminGetDTO>>> GetAllAdmin(int userId)
        {
            var user = await _dataContext.Users
                                        .Include(u => u.Notifications)
                                        .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return new ServiceResponse<IEnumerable<NotificationAdminGetDTO>>(404, "User has not been found.");
            }

           var notificationsDtos = user.Notifications?.Select(n =>
           _mapper.Map<NotificationAdminGetDTO>(n)
           /*new NotificationAdminGetDTO()
           {
               Id = n.Id,
               Message = n.Message,
               Date = n.Date,
               IsRead = n.IsRead,
               UserId = n.UserId,
               Type = n.Type,
               VideoId = n.VideoId,
               CommentId = n.CommentId,
           }*/);

           return ServiceResponse<IEnumerable<NotificationAdminGetDTO>>.OK(notificationsDtos);
        }

        public async Task<ServiceResponse> ToggleTrueIsRead(int notificationId, int userId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);
            // If the notification does not exists, return 404
            if(notification == null)
            {
                return new ServiceResponse(404, $"Notification with {notificationId} id does not exist.");
            }
            // If userIds do not match, return 403
            if (userId != -1 && notification.UserId != userId)
            {
                return new ServiceResponse(403, "Forbidden");
            }
            // If the notification has already been read, return 304
            if(notification.IsRead) 
            {
                return ServiceResponse.NotModified;
            }
            // Update the entity since it is already tracked via FindAsync()
            notification.IsRead = true;
            await _dataContext.SaveChangesAsync();
            return ServiceResponse.OK;
        }
    }
}
