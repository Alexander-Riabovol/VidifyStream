using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IHttpContextAccessor _accessor;

        public AuthService(IDataProtectionProvider dpp, IHttpContextAccessor accessor)
        { 
            _dataProtectionProvider = dpp;
            _accessor = accessor;
        }

        public void SignIn()
        {
            var protector = _dataProtectionProvider.CreateProtector("auth-cookie");
            _accessor.HttpContext!.Response.Headers.SetCookie = $"auth={protector.Protect("usr:test")}";
        }
    }
}
