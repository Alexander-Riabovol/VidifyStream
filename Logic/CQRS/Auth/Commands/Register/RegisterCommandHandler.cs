using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Services.Users;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Logic.CQRS.Auth.Queries.Login;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    public class RegisterCommandHandler : 
        IRequestHandler<RegisterCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;
        private readonly IUserService _userService;

        public RegisterCommandHandler(DataContext dataContext,
                                      IMapper mapper,
                                      ISender mediator,
                                      IUserService userService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mediator = mediator;
            _userService = userService;
        }

        public async Task<ServiceResponse<int>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                return new ServiceResponse<int>(409, "The provided email address is already associated with an existing user account.");
            }

            var registeredUser = _mapper.Map<User>(request);

            var response = await _userService.CreateUser(registeredUser);

            // Send Login query with MediatR
            var loginQuery = _mapper.Map<LoginQuery>(request);
            await _mediator.Send(loginQuery);

            // Send confrimation email here
            return ServiceResponse<int>.OK(response.Content);
        }
    }
}
