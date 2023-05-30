using Data.Dtos.User;

namespace Logic.Services.AuthService
{
    public interface IAuthService
    {
        const string AuthScheme = "cookie";
        Task LogIn(UserLoginDTO loginData);
        Task LogOut();
    }
}
