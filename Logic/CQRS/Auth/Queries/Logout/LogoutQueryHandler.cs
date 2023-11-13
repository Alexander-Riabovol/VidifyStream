using VidifyStream.Data.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Logout
{
    public class LogoutQueryHandler : IRequestHandler<LogoutQuery, ServiceResponse>
    {
        private readonly IHttpContextAccessor _accessor;

        public LogoutQueryHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<ServiceResponse> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            await _accessor.HttpContext!.SignOutAsync();
            return ServiceResponse.OK;
        }
    }
}
