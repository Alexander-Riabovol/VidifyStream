using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get.Admin
{
    public class GetUserAdminQueryHandler :
        IRequestHandler<GetUserAdminQuery, ServiceResponse<UserAdminGetDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public GetUserAdminQueryHandler(DataContext dataContext,
                                        IHttpContextAccessor accessor,
                                        IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserAdminGetDTO>> Handle(GetUserAdminQuery request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.IgnoreQueryFilters()
                                               .Include(u => u.Videos)
                                               .Include(u => u.Comments)
                                               .Include(u => u.Notifications)
                                               .FirstOrDefaultAsync(u => u.UserId == request.UserId,
                                                                    cancellationToken);

            if (user == null)
            {
                return new ServiceResponse<UserAdminGetDTO>
                    (404, $"User with ID {request.UserId} does not exist.");
            }


            var userAdminGetDTO = _mapper.Map<UserAdminGetDTO>(user);

            if (userAdminGetDTO.Status == Status.Admin)
            {
                if (_accessor.HttpContext!.RetriveUserId().Content != request.UserId)
                {
                    userAdminGetDTO.Password = "hidden";
                }
            }

            return ServiceResponse<UserAdminGetDTO>.OK(userAdminGetDTO);
        }
    }
}
