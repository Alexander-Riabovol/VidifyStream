using Data.Context;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace API.Middleware
{
    // Add a comment why we need to check User.Status field dynamicly and can not just store it in the cookie.
    public class StatusRequirementHandler : AuthorizationHandler<StatusRequirement>
    {
        private readonly DataContext _dataContext;

        public StatusRequirementHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // TO DO:
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

            // If Unauthenticated return 401
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

            // Extract User data from the database.
            var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == "id");
            var user = idClaim != null ? await _dataContext.Users.FindAsync(int.Parse(idClaim.Value)) : null;

            if (user == null)
            {
                response?.OnStarting(async () =>
                {
                    filterContext!.HttpContext.Response.StatusCode = 500;
                    var message = Encoding.ASCII.GetBytes("You have been authenticated but your user data could not be found. Try to log in again.");
                    await response.Body.WriteAsync(message, 0, message.Length);
                });
                context.Fail();
                return;
            }

            if(requirement.AllowedStatuses.Contains(user.Status))
            {
                context.Succeed(requirement);
            }
            else
            {
                // If does not meet requirements return 403
                response?.OnStarting(() =>
                {
                    filterContext!.HttpContext.Response.StatusCode = 403;
                    return Task.CompletedTask;
                });
                context.Fail();
            }
        }
    }
}
