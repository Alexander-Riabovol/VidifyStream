using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VidifyStream.Logic.CQRS.Auth.Common;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Login
{
    public class LoginQueryHandler :
        IRequestHandler<LoginQuery, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

        public LoginQueryHandler(DataContext dataContext,
                                 IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        public async Task<ServiceResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new ServiceResponse(404, "No user with provided email has been found in the database.");
            }
            if (user.Password != request.Password)
            {
                return new ServiceResponse(401, "The password is not correct.");
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.UserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, AuthScheme.Default);
            var principal = new ClaimsPrincipal(identity);

            await _accessor.HttpContext!.SignInAsync(AuthScheme.Default, principal);
            return ServiceResponse.OK;
        }
    }
}
