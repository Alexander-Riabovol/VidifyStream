using VidifyStream.Data.Context;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using VidifyStream.Logic.CQRS.Auth.Common;

namespace VidifyStream.API.Middleware
{
    /// <summary>
    /// Represents a handler for the <see cref="StatusRequirement"/> authorization requirement.
    /// </summary>
    public class StatusRequirementHandler : AuthorizationHandler<StatusRequirement>
    {
        private readonly DataContext _dataContext;

        public StatusRequirementHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // In this method we dynamicaly retrive user from the database according to id in his cookie.
        // The reason we do not store user's Status is that user might get banned before resetting his cookie.
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                             StatusRequirement requirement) 
        {
            var filterContext = context.Resource as DefaultHttpContext;
            var response = filterContext?.HttpContext.Response;

            // If Unauthenticated return 401
            if (context.User.Identity == null ||
                !context.User.Identity.IsAuthenticated || 
                context.User.Identity.AuthenticationType != AuthScheme.Default) 
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
