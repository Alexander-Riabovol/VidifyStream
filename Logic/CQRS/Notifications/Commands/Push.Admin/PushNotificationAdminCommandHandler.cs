using MapsterMapper;
using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Push.Admin
{
    public class PushNotificationAdminCommandHandler
        : IRequestHandler<PushNotificationAdminCommand, ServiceResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public PushNotificationAdminCommandHandler(IMapper mapper,
                                                   ISender mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ServiceResponse> Handle(PushNotificationAdminCommand request, CancellationToken cancellationToken)
        {
            var notification = _mapper.Map<Notification>(request.NotificationDto);

            return await _mediator.Send(new PushNotificationCommand(notification));
        }
    }
}
