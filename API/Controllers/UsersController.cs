using Data.Dtos.User;
using Data.Models;
using Logic.Services.UserService;
using Logic.Services.ValidationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidationService _validationService;

        public UsersController(IUserService userService, IValidationService validationService) 
        {
            _userService = userService;
            _validationService = validationService;
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
    }
}
