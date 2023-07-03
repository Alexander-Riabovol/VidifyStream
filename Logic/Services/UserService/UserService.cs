using Data.Context;
using Data.Dtos;
using Data.Dtos.User;
using Data.Models;
using Logic.Extensions;
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

        public async Task<ServiceResponse<int>> CreateUser(User user)
        {
            // Set the blank profile picture as default
            var scheme = _accessor.HttpContext!.Request.Scheme;
            var host = _accessor.HttpContext!.Request.Host.ToUriComponent();
            user.ProfilePictureUrls.Add($"{scheme}://{host}/api/download/blank");

            await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse<int>.OK(user.UserId);
        }

        public async Task<ServiceResponse<string>> UploadProfilePicture(UserProfilePicturePostDTO pfpDTO)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<string>(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);
            if(user == null)
            {
                return new ServiceResponse<string>(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
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
