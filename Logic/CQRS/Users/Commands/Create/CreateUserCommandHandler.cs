using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.CQRS.Users.Commands.Create
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

        public CreateUserCommandHandler(DataContext dataContext,
                                        IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Set the blank profile picture as default
            var scheme = _accessor.HttpContext!.Request.Scheme;
            var host = _accessor.HttpContext!.Request.Host.ToUriComponent();
            request.User.ProfilePictureUrls.Add($"{scheme}://{host}/api/download/blank");

            await _dataContext.AddAsync(request.User, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse<int>.OK(request.User.UserId);
        }
    }
}
