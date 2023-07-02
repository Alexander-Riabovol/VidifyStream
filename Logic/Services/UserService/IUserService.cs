using Data.Dtos;
using Data.Dtos.User;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> UploadProfilePicture(UserProfilePicturePostDTO pfpDTO);
    }
}