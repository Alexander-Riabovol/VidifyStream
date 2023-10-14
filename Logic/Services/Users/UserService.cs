using Data.Context;
using Data.Dtos;
using Data.Dtos.User;
using Data.Models;
using Logic.Extensions;
using Logic.Services.Files;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VidifyStream.Logic.CQRS.Auth.Common;

namespace Logic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UserService(DataContext dataContext,
                           IHttpContextAccessor accessor,
                           ILogger<UserService> logger,
                           IMapper mapper,
                           IFileService fileService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
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

        public async Task<ServiceResponse<UserGetMeDTO>> GetMe()
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<UserGetMeDTO>(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.Include(u => u.Videos)
                                               .FirstOrDefaultAsync(u => u.UserId == idResult.Content);

            if (user == null)
            {
                return new ServiceResponse<UserGetMeDTO>(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            var userDto = user.Adapt<UserGetMeDTO>();
            return ServiceResponse<UserGetMeDTO>.OK(userDto);
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
                if (_accessor.HttpContext!.RetriveUserId().Content != userId)
                {
                    userAdminGetDTO.Password = "hidden";
                }
            }

            return ServiceResponse<UserAdminGetDTO>.OK(userAdminGetDTO);
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

        public async Task<ServiceResponse> Put(UserPutDTO userPutDTO)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);

            if (user == null)
            {
                return new ServiceResponse(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            user = _mapper.Map(userPutDTO, user);

            _dataContext.Update(user);
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

        public async Task<ServiceResponse> DeleteMe()
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);

            if (user == null)
            {
                return new ServiceResponse(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }
            if (user.Status == Status.Admin)
            {
                return new ServiceResponse(403, "You can't delete your own account because you are an Admin.");
            }

            user.Status = Status.SelfDeleted;

            _dataContext.Remove(user);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> DeleteAdmin(UserAdminDeleteDTO userAdminDeleteDTO)
        {
            var user = await _dataContext.Users.IgnoreQueryFilters()
                                               .FirstOrDefaultAsync(u => u.UserId == userAdminDeleteDTO.UserId);

            if (user == null)
            {
                return new ServiceResponse(404, $"User with ID {userAdminDeleteDTO.UserId} was not found in the database.");
            }
            if (user.DeletedAt != null)
            {
                return ServiceResponse.NotModified;
            }
            if (user.Status == Status.Admin)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            user = _mapper.Map(userAdminDeleteDTO, user);

            _dataContext.Remove(user);
            await _dataContext.SaveChangesAsync();

            var idResult = _accessor.HttpContext!.RetriveUserId();
            var admin = await _dataContext.Users.FindAsync(idResult.Content);
            _logger.LogInformation($"Admin {{Name: {admin?.Name}, ID: {admin?.UserId}}} banned User {{Name: {user.Name}, Email: {user.Email}, ID: {user.UserId}}}.");
            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse<User>> AddAdminDebug()
        {
            int number = 0;
            User? user = new User();
            while(user != null)
            {
                number++;
                user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == $"admin{number}@test.com");
            }

            var result = await CreateUser(new User()
            {
                Name = $"Admin{number}",
                BirthDate = DateTime.Now.AddYears(-20),
                Email = $"admin{number}@test.com",
                Password = "admin",
                Status = Status.Admin,
            });

            if (result.IsError) return new ServiceResponse<User>(result.StatusCode, result.Message!);

            user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == $"admin{number}@test.com");

            var claims = new List<Claim>
            {
                new Claim("id", user!.UserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, AuthScheme.Default);
            var principal = new ClaimsPrincipal(identity);

            await _accessor.HttpContext!.SignInAsync(AuthScheme.Default, principal);

            return ServiceResponse<User>.OK(user);
        }
    }
}
