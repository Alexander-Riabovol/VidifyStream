using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Users.Commands.Delete.Admin
{
    public class DeleteUserAdminCommandHandler
        : IRequestHandler<DeleteUserAdminCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<DeleteUserAdminCommandHandler> _logger;
        private readonly IMapper _mapper;

        public DeleteUserAdminCommandHandler(DataContext dataContext,
                           IHttpContextAccessor accessor,
                           ILogger<DeleteUserAdminCommandHandler> logger,
                           IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(DeleteUserAdminCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.IgnoreQueryFilters()
                                               .FirstOrDefaultAsync(u =>
                                                u.UserId == request.UserDto.UserId);

            if (user == null)
            {
                return new ServiceResponse(
                    404, $"User with ID {request.UserDto.UserId} was not found in the database.");
            }
            if (user.DeletedAt != null)
            {
                return ServiceResponse.NotModified;
            }
            if (user.Status == Status.Admin)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            user = _mapper.Map(request.UserDto, user);

            _dataContext.Remove(user);
            await _dataContext.SaveChangesAsync(cancellationToken);

            var idResult = _accessor.HttpContext!.RetriveUserId();
            var admin = await _dataContext.Users.FindAsync(idResult.Content);
            _logger.LogInformation($"Admin {{Name: {admin?.Name}, ID: {admin?.UserId}}} banned User {{Name: {user.Name}, Email: {user.Email}, ID: {user.UserId}}}.");
            return ServiceResponse.OK;
        }
    }
}
