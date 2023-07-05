﻿using Logic.Hubs;
using Data.Context;
using Data.Dtos;
using Data.Dtos.Notification;
using Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;

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

            if (user.Notifications == null)
            {
                return ServiceResponse<IEnumerable<NotificationGetDTO>>.OK(null);
            }

            var result = new List<NotificationGetDTO>();

            foreach (var notification in user.Notifications)
            {
                result.Add(await _mapper.From(notification).AdaptToTypeAsync<NotificationGetDTO>());
            }

            return ServiceResponse<IEnumerable<NotificationGetDTO>>.OK(result);
        }

        public async Task<ServiceResponse<NotificationAdminGetDTO>> GetAdmin(int notificationId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);

            if(notification == null)
            {
                return new ServiceResponse<NotificationAdminGetDTO>(404, "Notification has not been found.");
            }

            var notificationDto = _mapper.Map<NotificationAdminGetDTO>(notification);
            return ServiceResponse<NotificationAdminGetDTO>.OK(notificationDto);
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

           var notificationsDtos = user.Notifications?.Select(n => _mapper.Map<NotificationAdminGetDTO>(n));

           return ServiceResponse<IEnumerable<NotificationAdminGetDTO>>.OK(notificationsDtos);
        }

        // TO DO: Write a second createAndSend function for sending notifications via Admin NotificationController
        // and rewrite this function with Notification as a parameter to avoid mapping.
        // (It will be used by server only anyway)
        public async Task<ServiceResponse> CreateAndSend(NotificationCreateDTO notificationCreateDto)
        {
            var notification = _mapper.Map<Notification>(notificationCreateDto);

            await _dataContext.AddAsync(notification);
            await _dataContext.SaveChangesAsync();

            var notificationGetDto = await _mapper.From(notification).AdaptToTypeAsync<NotificationGetDTO>();
            // broadcast the new notification to a user group to whom it addressed
            await _notificationsHub.Clients.Group($"push-{notification.UserId}")
                .SendAsync("push-notifications", new List<NotificationGetDTO> { notificationGetDto });

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> ToggleTrueIsRead(int notificationId, int userId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);
            // If the notification does not exists, return 404
            if(notification == null)
            {
                return new ServiceResponse(404, $"Notification with ID {notificationId} does not exist.");
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

        public async Task<ServiceResponse> Delete(int notificationId, int callerId)
        {
            var notification = await _dataContext.Notifications.FindAsync(notificationId);
            // If the notification does not exists, return 404
            if (notification == null)
            {
                return new ServiceResponse(404, $"Notification with ID {notificationId} does not exist.");
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
    }
}
