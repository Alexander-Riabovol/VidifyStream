using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.Hubs;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Push
{
    public class PushNotificationCommandHandler :
        IRequestHandler<PushNotificationCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHubContext<NotificationsHub> _notificationsHub;
        private readonly IMapper _mapper;

        public PushNotificationCommandHandler(DataContext dataContext,
                                              IHubContext<NotificationsHub> notificationsHub,
                                              IMapper mapper)
        {
            _dataContext = dataContext;
            _notificationsHub = notificationsHub;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(PushNotificationCommand request, CancellationToken cancellationToken)
        {
            await _dataContext.AddAsync(request.Notification, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            var notificationGetDto = await _mapper.From(request.Notification)
                                                  .AdaptToTypeAsync<NotificationGetDTO>();
            // broadcast the new notification to a user group to whom it is addressed
            await _notificationsHub.Clients
                                   .Group($"push-{request.Notification.UserId}")
                                   .SendAsync("push-notifications",
                                              new List<NotificationGetDTO> { notificationGetDto },
                                              cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
