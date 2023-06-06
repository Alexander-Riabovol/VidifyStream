using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationsHub : Hub
    {
        private static int pingCount { get; set; }
        public async override Task OnConnectedAsync()
        {
            var ctx = Context.GetHttpContext();
            var authCookie = ctx?.Request.Headers["Cookie"][0];
            
            // if there is no auth cookie, close the connection
            if(authCookie == null || authCookie == "")
            {
                ctx?.Connection.RequestClose();
            }

            await base.OnConnectedAsync();
        }
        public async Task Ping()
        {
            pingCount++;
            await Clients.All.SendAsync("pingc", pingCount);
        }
    }
}
