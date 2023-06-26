using Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse> UploadProfilePicture(IFormFile file);
    }
}