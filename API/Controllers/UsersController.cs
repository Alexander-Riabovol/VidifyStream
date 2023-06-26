using Data.Dtos.User;
using Logic.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("pfp")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await _userService.UploadProfilePicture(file);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
