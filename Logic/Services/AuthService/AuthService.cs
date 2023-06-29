using Data.Context;
using Data.Dtos;
using Data.Dtos.User;
using Data.Models;
using Logic.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Logic.Services.AuthService
{
    //TO DO: Add Register method + RegisterDTO & LoginDTO validation
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public AuthService(DataContext dataContext, IHttpContextAccessor accessor, IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Register(UserRegisterDTO registerData)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == registerData.Email);
            if (user != null)
            {
                return new ServiceResponse(409, "The provided email address is already associated with an existing user account.");
            }

            var registeredUser = _mapper.Map<User>(registerData);

            await _dataContext.AddAsync(registeredUser);
            await _dataContext.SaveChangesAsync();

            var loginData = _mapper.Map<UserLoginDTO>(registerData);
            await LogIn(loginData);

            // Send confrimation email here
            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> LogIn(UserLoginDTO loginData)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email);

            if(user == null) 
            {
                return new ServiceResponse(404, "No user with provided email has been found in the database.");
            }
            if (user.Password != loginData.Password) 
            {
                return new ServiceResponse(401, "The password is not correct.");
            }

            //var ip = _accessor.HttpContext!.DeriveIp();

            var claims = new List<Claim>
            {
                new Claim("id", user.UserId.ToString())
            };
            var identity = new ClaimsIdentity(claims, IAuthService.AuthScheme);
            var principal = new ClaimsPrincipal(identity);

            await _accessor.HttpContext!.SignInAsync(IAuthService.AuthScheme, principal);
            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> LogOut()
        {
            await _accessor.HttpContext!.SignOutAsync();
            return ServiceResponse.OK;
        }
    }
}
