using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.GetAll.My
{
    public class GetAllMyNotificationsQueryHandler
        : IRequestHandler<GetAllMyNotificationsQuery, ServiceResponse<IEnumerable<NotificationGetDTO>>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public GetAllMyNotificationsQueryHandler(DataContext dataContext,
                                                 IHttpContextAccessor accessor,
                                                 IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<NotificationGetDTO>>> Handle(GetAllMyNotificationsQuery request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) 
                return new ServiceResponse<IEnumerable<NotificationGetDTO>>(idResult.StatusCode,
                                                                            idResult.Message!);

            var user = await _dataContext.Users
                                         .Include(u => u.Notifications)
                                         .FirstOrDefaultAsync(u => u.UserId == idResult.Content);

            if (user == null)
            {
                return new ServiceResponse<IEnumerable<NotificationGetDTO>>(
                    500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            if (request.UnreadOnly)
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
    }
}
