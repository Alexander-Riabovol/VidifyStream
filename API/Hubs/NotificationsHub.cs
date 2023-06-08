using Logic;
using Logic.Services.AuthService;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly AppData _data;
        private static int pingCount { get; set; }

        public NotificationsHub(AppData data) : base()
        {
            _data = data;
        }

        public async override Task OnConnectedAsync()
        {
            var identity = Context.User?.Identity;

            // if not authenticated, close the connection
            if (identity == null || !identity.IsAuthenticated 
                || identity.AuthenticationType != IAuthService.AuthScheme)
            {
                Context.Abort();
                return;
            }
            // retrive the id from the claims. It can't be null because user is authenticated already.
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            // add user to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"push-{userId}");
            // check if there are any other connected users in the group
            if (!_data.ActiveNotificationUsers.ContainsKey(userId))
            {
                // add sending unread notifications here
                _data.ActiveNotificationUsers.Add(userId, 0);
            }
            _data.ActiveNotificationUsers[userId]++;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // add to the logic connectionId in the future

            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;

            _data.ActiveNotificationUsers[userId]--;
            if(_data.ActiveNotificationUsers[userId] == 0)
            {
                _data.ActiveNotificationUsers.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task Ping()
        {
            pingCount++;
            var id = Context.User!.Claims.First().Value;
            await Clients.Group($"push-{id}").SendAsync("groupPing", id, _data.ActiveNotificationUsers[id], pingCount);
        }
    }
}
