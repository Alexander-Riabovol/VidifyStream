using Data.Dtos;
using Data.Dtos.User;
using Data.Models;

namespace Logic.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> UploadProfilePicture(UserProfilePicturePostDTO pfpDTO);
        Task<ServiceResponse<int>> CreateUser(User user);
        Task<ServiceResponse<UserGetDTO>> Get(int userId);
        Task<ServiceResponse<UserGetMeDTO>> GetMe();
        Task<ServiceResponse<UserAdminGetDTO>> GetAdmin(int userId);
        Task<ServiceResponse> Put(UserPutDTO userPutDTO);
        Task<ServiceResponse> DeleteMe();
        Task<ServiceResponse> DeleteAdmin(UserAdminDeleteDTO userAdminDeleteDTO);
    }
}