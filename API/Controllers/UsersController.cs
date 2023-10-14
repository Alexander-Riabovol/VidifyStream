using Data.Dtos.User;
using Logic.Services.Users;
using Logic.Services.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Logic.CQRS.Auth.Queries.Logout;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISender _mediator;
        private readonly IValidationService _validationService;

        public UsersController(IUserService userService,
                               ISender mediator,   
                               IValidationService validationService) 
        {
            _userService = userService;
            _mediator = mediator;
            _validationService = validationService;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize("user+")]
        public async Task<ActionResult<UserGetMeDTO>> GetMyProfile()
        {
            var response = await _userService.GetMe();
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserGetDTO>> Get(int userId)
        {
            var response = await _userService.Get(userId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpGet]
        [Route("admin/{userId}")]
        [Authorize("admin-only")]
        public async Task<ActionResult<UserAdminGetDTO>> GetAdmin(int userId)
        {
            var response = await _userService.GetAdmin(userId);
            if (response.IsError)
            {
                return StatusCode(response.StatusCode, response.Message);
            }
            return Ok(response.Content);
        }

        [HttpPut]
        [Authorize("user+")]
        public async Task<IActionResult> Put(UserPutDTO userPutDTO)
        {
            var validationResult = _validationService.Validate(userPutDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _userService.Put(userPutDTO);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost]
        [Route("pfp")]
        [Authorize("user+")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadProfilePicture([FromForm]UserProfilePicturePostDTO pfpDTO)
        {
            var validationResult = _validationService.Validate(pfpDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _userService.UploadProfilePicture(pfpDTO);
            if(result.IsError) 
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            return Ok(result.Content);
        }

        [HttpDelete]
        [Route("admin/")]
        [Authorize("admin-only")]
        public async Task<IActionResult> DeleteAdmin(UserAdminDeleteDTO userAdminDeleteDTO)
        {
            var validationResult = _validationService.Validate(userAdminDeleteDTO);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _userService.DeleteAdmin(userAdminDeleteDTO);
            return StatusCode(result.StatusCode, result.Message);
        }
        [HttpDelete]
        [Route("profile")]
        [Authorize("user+")]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var result = await _userService.DeleteMe();
            if(!result.IsError) 
            {
                await _mediator.Send(new LogoutQuery());
            }
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
