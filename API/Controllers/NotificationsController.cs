using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.Services.Notifications;
using VidifyStream.Logic.Services.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VidifyStream.Logic.CQRS.Notifications.Commands.Push.Admin;

namespace VidifyStream.API.Controllers
{
    [Route("api/notifications")]
    [Authorize(Policy = "admin-only")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISender _mediator;

        private readonly INotificationService _notificationService;

        public NotificationsController(ISender mediator,
                                       INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("{notificationId}")]
        public async Task<ActionResult<NotificationAdminGetDTO>> Get(int notificationId)
        {
            var response = await _notificationService.GetAdmin(notificationId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<List<NotificationAdminGetDTO>>> GetAllByUserId(int userId)
        {
            var response = await _notificationService.GetAllAdmin(userId);
            if(response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPost]
        public async Task<IActionResult> Post(NotificationAdminCreateDTO notificationDto)
        {
            var response = await _mediator.Send(new PushNotificationAdminCommand(notificationDto));
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
