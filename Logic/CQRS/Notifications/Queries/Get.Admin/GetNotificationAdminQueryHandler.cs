using MapsterMapper;
using MediatR;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;

namespace VidifyStream.Logic.CQRS.Notifications.Queries.Get.Admin
{
    public class GetNotificationAdminQueryHandler
        : IRequestHandler<GetNotificationAdminQuery, ServiceResponse<NotificationAdminGetDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetNotificationAdminQueryHandler(DataContext dataContext,
                                                IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<NotificationAdminGetDTO>> Handle(GetNotificationAdminQuery request, CancellationToken cancellationToken)
        {
            var notification = await _dataContext.Notifications.FindAsync(request.NotificationId);

            if (notification == null)
            {
                return new ServiceResponse<NotificationAdminGetDTO>(
                    404, "Notification has not been found.");
            }

            var notificationDto = _mapper.Map<NotificationAdminGetDTO>(notification);
            return ServiceResponse<NotificationAdminGetDTO>.OK(notificationDto);
        }
    }
}
