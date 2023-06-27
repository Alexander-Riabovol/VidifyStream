using Data.Context;
using Data.Dtos;
using Logic.Services.FileService;
using Microsoft.AspNetCore.Http;

namespace Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public UserService(DataContext dataContext, IFileService fileService) 
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<string>> UploadProfilePicture(IFormFile file)
        {
            var result = await _fileService.Upload(file);
            if(!result.IsError) return ServiceResponse<string>.OK(result.Content);
            return new ServiceResponse<string>(result.StatusCode, result.Message!);
        }
    }
}
