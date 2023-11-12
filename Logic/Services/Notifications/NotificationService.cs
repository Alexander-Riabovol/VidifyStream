using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public NotificationService(DataContext dataContext,
                                   IHttpContextAccessor accessor,
                                   IMapper mapper) 
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> GetAllMy(bool onlyUnread)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<IEnumerable<NotificationGetDTO>> (idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users
                                         .Include(u => u.Notifications)
                                         .FirstOrDefaultAsync(u => u.UserId == idResult.Content);

            if (user == null)
            {
                return new ServiceResponse<IEnumerable<NotificationGetDTO>>(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
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
    }
}
