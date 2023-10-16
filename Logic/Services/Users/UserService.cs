using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Auth.Common;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext dataContext,
                           IHttpContextAccessor accessor,
                           ILogger<UserService> logger)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
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
