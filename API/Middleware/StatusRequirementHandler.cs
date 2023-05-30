using Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Middleware
{
    public class StatusRequirementHandler : AuthorizationHandler<StatusRequirement>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

        public StatusRequirementHandler(DataContext dataContext, IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             StatusRequirement requirement) {
            context.Succeed(requirement);
            //context.Fail();

            // TO DO:
            // Add ef core support (check status)
            // Check if the ip is usual
            // The idea is that if the ip is not usual but the user have the cookie,
            // than maybe it is someone who has stolen the cookie, so it is better to redirect to login

            // Add different status codes for different outcomes.
            var filterContext = context.Resource as DefaultHttpContext;
            var response = filterContext?.HttpContext.Response;
            response?.OnStarting(() =>
            {
                filterContext!.HttpContext.Response.StatusCode = 403;
                // If I need to pass a message
                // await response.Body.WriteAsync(message, 0, message.Length);
                return Task.CompletedTask;
            });
        }
    }
}
