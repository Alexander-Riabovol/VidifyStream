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
            await _authService.LogIn(loginDTO);
            return Ok();
        }

        [HttpGet]
        [Route("api/logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogOut();
            return Ok();
        }

        [HttpGet]
        [Route("api/test")]
        [Authorize(Policy = "admin-only")]
        public IActionResult TestAdmin()
        {
            return Ok("you're admin");
        }
    }
}
