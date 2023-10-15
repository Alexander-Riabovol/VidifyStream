using VidifyStream.Data.Dtos.User;
using VidifyStream.Logic.Services.Validation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Logic.CQRS.Auth.Commands.Register;
using VidifyStream.Logic.CQRS.Auth.Queries.Login;
using VidifyStream.Logic.CQRS.Auth.Queries.Logout;

namespace VidifyStream.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public AuthController(IMapper mapper,
                              ISender mediator) 
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login(UserLoginDTO loginData)
        {
            var query = _mapper.Map<LoginQuery>(loginData);
            var result = await _mediator.Send(query);

            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<int>> Register(UserRegisterDTO registerData)
        {
            var command = _mapper.Map<RegisterCommand>(registerData);
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            return Ok(result.Content);
        }

        [HttpGet]
        [Route("api/logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutQuery());
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
