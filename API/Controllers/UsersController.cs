using Data.Dtos.User;
using Logic.Services.AuthService;
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
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IValidationService _validationService;

        public UsersController(IAuthService authService,
                               IUserService userService,
                               IValidationService validationService) 
        {
            _authService = authService;
            _userService = userService;
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
                await _authService.LogOut();
            }
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
