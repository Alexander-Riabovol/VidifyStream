using Data.Dtos.Comment;
using Data.Dtos.User;
using Logic.Services.UserService;
using Logic.Services.ValidationService;
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

        [HttpPost]
        [Route("pfp")]
        public async Task<ActionResult<string>> UploadProfilePicture(IFormFile file, 
                                                                     CancellationToken cancellationToken)
        {
            var validationResult = _validationService.Validate(file, "image");
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _userService.UploadProfilePicture(file);
            if(result.IsError) 
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            return Ok(result.Content);
        }
    }
}
