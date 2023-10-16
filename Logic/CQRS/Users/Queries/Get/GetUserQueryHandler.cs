using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Files;
using VidifyStream.Logic.Services.Users;

namespace VidifyStream.Logic.CQRS.Users.Queries.Get
{
    public class GetUserQueryHandler :
        IRequestHandler<GetUserQuery, ServiceResponse<UserGetDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(DataContext dataContext,
                                   IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserGetDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.Include(u => u.Videos)
                             .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (user == null)
            {
                return new ServiceResponse<UserGetDTO>(404, $"User with ID {request.UserId} does not exist.");
            }

            var userGetDTO = _mapper.Map<UserGetDTO>(user);
            return ServiceResponse<UserGetDTO>.OK(userGetDTO);
        }
    }
}
