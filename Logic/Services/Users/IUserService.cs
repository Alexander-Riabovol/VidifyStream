using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.Services.Users
{
    /// <summary>
    /// Represents a service for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        Task<ServiceResponse<int>> CreateUser(User user);
        Task<ServiceResponse<User>> AddAdminDebug();
    }
}