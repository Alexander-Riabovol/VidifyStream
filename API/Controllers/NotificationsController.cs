using Data.Dtos.Notification;
using FluentValidation;
using Logic.Services.NotificationService;
using Logic.Services.ValidationService;
using Mapster;
using MapsterMapper;
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
        private readonly IValidationService _validationService;

        public NotificationsController(INotificationService notificationService,
                                       IValidationService validationService)
        {
            _notificationService = notificationService;
            _validationService = validationService;
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
            var validationResult = await _validationService.ValidateAsync(notificationDto);
            if(validationResult.IsError) 
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var response = await _notificationService.CreateAndSend(notificationDto.Adapt<NotificationCreateDTO>());
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
