using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Logic.CQRS.Notifications.Commands.Push.Admin;
using VidifyStream.Logic.CQRS.Notifications.Queries.Get.Admin;
using VidifyStream.Logic.CQRS.Notifications.Queries.GetAll.Admin;

namespace VidifyStream.API.Controllers
{
    [Route("api/notifications")]
    [Authorize(Policy = "admin-only")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISender _mediator;

        public NotificationsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{notificationId}")]
        public async Task<ActionResult<NotificationAdminGetDTO>> Get(int notificationId)
        {
            var response = await _mediator.Send(new GetNotificationAdminQuery(notificationId));
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
            var response = await _mediator.Send(new GetAllNotificationsAdminQuery(userId));
            if (response.IsError)
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
