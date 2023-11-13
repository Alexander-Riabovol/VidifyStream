using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Auth.Common;
using VidifyStream.Logic.CQRS.Users.Commands.Create;

namespace VidifyStream.Logic.CQRS.Users.Commands.Debug.AddAdmin
{
    public class DebugAddAdminCommandHandler
        : IRequestHandler<DebugAddAdminCommand, ServiceResponse<User>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISender _mediator;

        public DebugAddAdminCommandHandler(DataContext dataContext,
                                           IHttpContextAccessor accessor,
                                           ISender mediator)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mediator = mediator;
        }

        public async Task<ServiceResponse<User>> Handle(DebugAddAdminCommand request, CancellationToken cancellationToken)
        {
            int number = 0;
            User? user = new User();
            while (user != null)
            {
                number++;
                user = await _dataContext.Users
                                         .FirstOrDefaultAsync(u => u.Email == $"admin{number}@test.com",
                                                              cancellationToken);
            }

            var result = await _mediator.Send(new CreateUserCommand(new User()
            {
                Name = $"Admin{number}",
                BirthDate = DateTime.Now.AddYears(-20),
                Email = $"admin{number}@test.com",
                Password = "admin",
                Status = Status.Admin,
            }));

            if (result.IsError) return new ServiceResponse<User>(result.StatusCode, result.Message!);

            user = await _dataContext.Users
                                     .FirstOrDefaultAsync(u => u.Email == $"admin{number}@test.com",
                                                          cancellationToken);

            var claims = new List<Claim>
            {
                new Claim("id", user!.UserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, AuthScheme.Default);
            var principal = new ClaimsPrincipal(identity);

            await _accessor.HttpContext!.SignInAsync(AuthScheme.Default, principal);

            return ServiceResponse<User>.OK(user);
        }
    }
}
