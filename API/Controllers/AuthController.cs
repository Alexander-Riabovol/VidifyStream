﻿using Data.Dtos.User;
using Logic.Services.Validation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidifyStream.Logic.CQRS.Auth.Commands.Register;
using VidifyStream.Logic.CQRS.Auth.Queries.Login;
using VidifyStream.Logic.CQRS.Auth.Queries.Logout;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;
        private readonly IValidationService _validationService;

        public AuthController(IMapper mapper,
                              ISender mediator,
                              IValidationService validationService) 
        {
            _mapper = mapper;
            _mediator = mediator;
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

            var query = _mapper.Map<LoginQuery>(loginData);
            var result = await _mediator.Send(query);

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
