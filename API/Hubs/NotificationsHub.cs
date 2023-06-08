using Logic;
using Logic.Services.AuthService;
using Logic.Services.NotificationService;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly AppData _data;
        private readonly INotificationService _notificationService;
        private static int pingCount { get; set; }

        public NotificationsHub(AppData data, INotificationService notificationService) : base()
        {
            _data = data;
            _notificationService = notificationService;
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
                var response = await _notificationService.GetAll(int.Parse(userId));
                var incomingNotifications = response.Content;
                if(!response.IsError && incomingNotifications != null)
                {
                    await Clients.Caller.SendAsync("broadcast-notifications", incomingNotifications);
                }
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
