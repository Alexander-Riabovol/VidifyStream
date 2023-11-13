using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.GetAll.Admin
{
    public class GetAllNotificationsAdminQueryHandler
        : IRequestHandler<GetAllNotificationsAdminQuery, ServiceResponse<IEnumerable<NotificationAdminGetDTO>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllNotificationsAdminQueryHandler(DataContext dataContext,
                                                    IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<NotificationAdminGetDTO>>> Handle(GetAllNotificationsAdminQuery request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                             .Include(u => u.Notifications)
                             .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (user == null)
            {
                return new ServiceResponse<IEnumerable<NotificationAdminGetDTO>>(
                    404, "User has not been found.");
            }

            var notificationsDtos = user.Notifications?.Select(_mapper.Map<NotificationAdminGetDTO>);

            return ServiceResponse<IEnumerable<NotificationAdminGetDTO>>.OK(notificationsDtos);
        }
    }
}
