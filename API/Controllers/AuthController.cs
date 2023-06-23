using Data.Dtos.User;
using Data.Models;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            var result = await _authService.LogIn(loginDTO);
            return StatusCode(result.StatusCode, result.Message);
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
