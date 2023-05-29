using Data.Dtos.User;

namespace Logic.Services.AuthService
{
    public interface IAuthService
    {
        const string AuthScheme = "cookie";
        Task SignIn(UserLoginDTO loginData);
    }
}
