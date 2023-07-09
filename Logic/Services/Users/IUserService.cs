using Data.Dtos;
using Data.Dtos.User;
using Data.Models;

namespace Logic.Services.Users
{
    /// <summary>
    /// Represents a service for managing users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a <see cref="User"/> by their ID.
        /// </summary>
        Task<ServiceResponse<UserGetDTO>> Get(int userId);
        /// <summary>
        /// Retrieves the current <see cref="User"/>.
        /// </summary>
        Task<ServiceResponse<UserGetMeDTO>> GetMe();
        /// <summary>
        /// Retrieves all info about <see cref="User"/> by their ID.
        /// </summary>
        Task<ServiceResponse<UserAdminGetDTO>> GetAdmin(int userId);
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        Task<ServiceResponse<int>> CreateUser(User user);
        /// <summary>
        /// Updates a <see cref="User"/>'s information.
        /// </summary>
        Task<ServiceResponse> Put(UserPutDTO userPutDTO);
        /// <summary>
        /// Uploads a profile picture for the current <see cref="User"/>.
        /// </summary>
        Task<ServiceResponse<string>> UploadProfilePicture(UserProfilePicturePostDTO pfpDTO);
        /// <summary>
        /// Deletes the current <see cref="User"/>.
        /// </summary>
        Task<ServiceResponse> DeleteMe();
        /// <summary>
        /// Deletes a <see cref="User"/> by it's ID.
        /// </summary>
        Task<ServiceResponse> DeleteAdmin(UserAdminDeleteDTO userAdminDeleteDTO);
        Task<ServiceResponse<User>> AddAdminDebug();
    }
}