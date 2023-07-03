using Data.Dtos.User;
using Logic.Services.AuthService;
using Logic.Services.ValidationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidationService _validationService;

        public AuthController(IAuthService authService, IValidationService validationService) 
        {
            _authService = authService;
            _validationService = validationService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login(UserLoginDTO loginData)
        {
            var validationResult = _validationService.Validate(loginData);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _authService.LogIn(loginData);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<int>> Register(UserRegisterDTO registerData)
        {
            var validationResult = _validationService.Validate(registerData);
            if (validationResult.IsError)
            {
                if (validationResult.Content == null) return StatusCode(validationResult.StatusCode, validationResult.Message);
                else return ValidationProblem(validationResult.Content);
            }

            var result = await _authService.Register(registerData);
            if(result.IsError)
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            return Ok(result.Content);
        }

        [HttpGet]
        [Route("api/logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogOut();
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
