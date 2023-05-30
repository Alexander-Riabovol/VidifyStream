using Data.Context;
using Data.Dtos.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Logic.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

        public AuthService(DataContext dataContext, IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        public async Task LogIn(UserLoginDTO loginData)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Email == loginData.Email);

            //Add Not Found
            if(user == null) { return; }

            //Add Forbid Access
            if (user.Password != loginData.Password) { return; }

            var claims = new List<Claim>
            {
                new Claim("id", user.UserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, IAuthService.AuthScheme);
            var principal = new ClaimsPrincipal(identity);

            await _accessor.HttpContext!.SignInAsync(IAuthService.AuthScheme, principal);
        }

        public async Task LogOut()
        {
            await _accessor.HttpContext!.SignOutAsync();
        }
    }
}
