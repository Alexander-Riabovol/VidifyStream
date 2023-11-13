using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get.Me
{
    public class GetUserMeQueryHandler :
        IRequestHandler<GetUserMeQuery, ServiceResponse<UserGetMeDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public GetUserMeQueryHandler(DataContext dataContext,
                                     IHttpContextAccessor accessor,
                                     IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserGetMeDTO>> Handle(GetUserMeQuery request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<UserGetMeDTO>(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.Include(u => u.Videos)
                                               .FirstOrDefaultAsync(u => u.UserId == idResult.Content);

            if (user == null)
            {
                return new ServiceResponse<UserGetMeDTO>(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            var userDto = _mapper.Map<UserGetMeDTO>(user);
            return ServiceResponse<UserGetMeDTO>.OK(userDto);
        }
    }
}
