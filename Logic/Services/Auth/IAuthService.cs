using Data.Dtos;
using Data.Dtos.User;

namespace Logic.Services.Auth
{
    /// <summary>
    /// Service class responsible for user authentication and authorization.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Represents the authentication scheme used for cookie-based authentication.
        /// </summary>
        const string AuthScheme = "cookie";
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="registerData">The user registration data.</param>
        /// <returns>A service response containing the registered user's ID on success, or an error response if registration fails.</returns>
        Task<ServiceResponse<int>> Register(UserRegisterDTO registerData);
        /// <summary>
        /// Logs in a user with the provided login data.
        /// </summary>
        /// <param name="loginData">The user login data.</param>
        /// <returns>A service response indicating the result of the login operation.</returns>
        Task<ServiceResponse> LogIn(UserLoginDTO loginData);
        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>A service response indicating the result of the logout operation.</returns>
        Task<ServiceResponse> LogOut();
    }
}
