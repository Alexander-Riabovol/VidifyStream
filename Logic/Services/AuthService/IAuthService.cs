using Data.Dtos;
using Data.Dtos.User;

namespace Logic.Services.AuthService
{
    public interface IAuthService
    {
        const string AuthScheme = "cookie";
        Task<ServiceResponse> LogIn(UserLoginDTO loginData);
        Task<ServiceResponse> LogOut();
    }
}
