using Data.Context;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Middleware
{
    public class StatusRequirementHandler : AuthorizationHandler<StatusRequirement>
    {
        private readonly DataContext _dataContext;

        public StatusRequirementHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // TO DO:
        // Add ef core support (check status)
        // Check if the ip is usual
        // var ip = filterContext?.HttpContext.Connection.RemoteIpAddress;
        // The idea is that if the ip is not usual but the user have the cookie,
        // than maybe it is someone who has stolen the cookie, so it is better to redirect to login
        // maybe we need something like a session also
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             StatusRequirement requirement) 
        {
            var filterContext = context.Resource as DefaultHttpContext;
            var response = filterContext?.HttpContext.Response;
            // If Anauthenticated return 401
            if (context.User.Identity == null ||
                !context.User.Identity.IsAuthenticated || 
                context.User.Identity.AuthenticationType != IAuthService.AuthScheme) 
            {
                response?.OnStarting(() =>
                {
                    filterContext!.HttpContext.Response.StatusCode = 401;
                    return Task.CompletedTask;
                });
                context.Fail();
                return;
            }
            
            // Add different status codes for different outcomes.
            
            response?.OnStarting(() =>
            {
                filterContext!.HttpContext.Response.StatusCode = 403;
                // If I need to pass a message
                // await response.Body.WriteAsync(message, 0, message.Length);
                return Task.CompletedTask;
            });

            context.Succeed(requirement);
        }
    }
}
