using Logic.Services.AuthService;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationsHub : Hub
    {
        private static int pingCount { get; set; }
        public override Task OnConnectedAsync()
        {
            var ctx = Context.GetHttpContext();
            var identity = ctx?.User.Identity;

            // if not authenticated, close the connection
            if (identity == null || !identity.IsAuthenticated 
                || identity.AuthenticationType != IAuthService.AuthScheme)
            {
                ctx?.Abort();
            }

            return base.OnConnectedAsync();
        }
        public async Task Ping()
        {
            pingCount++;
            await Clients.All.SendAsync("pingc", pingCount);
        }
    }
}
