using Data.Dtos.Notification;
using Logic.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/notifications")]
    [Authorize(Policy = "admin-only")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
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
        public async Task<IActionResult> Post(NotificationCreateDTO notificationDto)
        {
            var response = await _notificationService.CreateAndSend(notificationDto);
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
