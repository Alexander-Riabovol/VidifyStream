using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.User;
using Data.Models;
using Logic.Extensions;
using Logic.Services.FileService;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public UserService(DataContext dataContext,
                           IFileService fileService,
                           IHttpContextAccessor accessor,
                           IMapper mapper)
        {
            _dataContext = dataContext;
            _fileService = fileService;
            _accessor = accessor;
            _mapper = mapper;
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

        public async Task<ServiceResponse<UserGetDTO>> Get(int userId)
        {
            var user = await _dataContext.Users.Include(u => u.Videos)
                                         .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) 
            {
                return new ServiceResponse<UserGetDTO>(404, $"User with ID {userId} does not exist.");
            }

            var userGetDTO = user.Adapt<UserGetDTO>();
            return ServiceResponse<UserGetDTO>.OK(userGetDTO);
        }

        public async Task<ServiceResponse<UserAdminGetDTO>> GetAdmin(int userId)
        {
            var user = await _dataContext.Users.IgnoreQueryFilters()
                                               .Include(u => u.Videos)
                                               .Include(u => u.Comments)
                                               .Include(u => u.Notifications)
                                               .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return new ServiceResponse<UserAdminGetDTO>(404, $"User with ID {userId} does not exist.");
            }


            var userAdminGetDTO = user.Adapt<UserAdminGetDTO>();

            if (userAdminGetDTO.Status == Status.Admin)
            {
                if(_accessor.HttpContext!.RetriveUserId().Content != userId)
                {
                    userAdminGetDTO.Password = "hidden";
                }
            }

            return ServiceResponse<UserAdminGetDTO>.OK(userAdminGetDTO);
        }

        public async Task<ServiceResponse> Put(UserPutDTO userPutDTO)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<string>(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);

            if (user == null)
            {
                return new ServiceResponse(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            user = _mapper.Map(userPutDTO, user);

            _dataContext.Remove(user);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
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
