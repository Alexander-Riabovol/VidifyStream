﻿using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Read
{
    public class ReadNotificationCommandHandler
        : IRequestHandler<ReadNotificationCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

        public ReadNotificationCommandHandler(DataContext dataContext,
                                              IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        public async Task<ServiceResponse> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _dataContext.Notifications.FindAsync(request.NotificationId);
            // If the notification does not exists, return 404
            if (notification == null)
            {
                return new ServiceResponse(404, $"Notification with ID {request.NotificationId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) 
                return new ServiceResponse<IEnumerable<NotificationGetDTO>>(idResult.StatusCode,
                                                                            idResult.Message!);

            // If userIds do not match, return 403
            if (notification.UserId != idResult.Content)
            {
                return new ServiceResponse(403, "Forbidden");
            }
            // If the notification has already been read, return 304
            if (notification.IsRead)
            {
                return ServiceResponse.NotModified;
            }
            // Update the entity since it is already tracked via FindAsync()
            notification.IsRead = true;
            await _dataContext.SaveChangesAsync(cancellationToken);
            return ServiceResponse.OK;
        }
    }
}
