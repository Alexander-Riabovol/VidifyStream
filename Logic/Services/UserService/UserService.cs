using Data.Context;
using Data.Dtos;
using Data.Dtos.User;
using Data.Models;
using Logic.Services.FileService;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _accessor;

        public UserService(DataContext dataContext,
                           IFileService fileService,
                           IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _fileService = fileService;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<string>> UploadProfilePicture(UserProfilePicturePostDTO pfpDTO)
        {
            var userId = int.Parse(_accessor.HttpContext!.User!.Claims.First(c => c.Type == "id")!.Value);

            var user = await _dataContext.Users.FindAsync(userId);

            if(user == null)
            {
                return new ServiceResponse<string>(500, $"Unknown error occured: a user with {userId} was not found.");
            }

            var fileUploadResult = await _fileService.Upload(pfpDTO.File);
            if(fileUploadResult.IsError)
            {
                return new ServiceResponse<string>(fileUploadResult.StatusCode, fileUploadResult.Message!);
            }

            user.ProfilePictureUrls.Insert(0, fileUploadResult.Content!);

            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse<string>.OK(user.ProfilePictureUrls.First());
        }
    }
}
