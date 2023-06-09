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

            // If not authenticated, close the connection
            if (identity == null || !identity.IsAuthenticated 
                || identity.AuthenticationType != IAuthService.AuthScheme)
            {
                Context.Abort();
                return;
            }
            // Retrive the id from the claims. It can't be null because user is authenticated already.
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            // Add user to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"push-{userId}");
            // Check if there are any other connected users in the group
            if (!_data.ActiveNotificationUsers.ContainsKey(userId))
            {
                // Get All notifications (unread by default)
                var response = await _notificationService.GetAll(int.Parse(userId));
                var incomingNotifications = response.Content;
                // If there is any, send notifications to the user
                if(!response.IsError && incomingNotifications != null)
                {
                    await Clients.Caller.SendAsync("broadcast-notifications", incomingNotifications);
                }
                // Add the user into the active users dictionary
                _data.ActiveNotificationUsers.Add(userId, 0);
            }
            _data.ActiveNotificationUsers[userId]++;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // add to the logic connectionId in the future
            // there can be an exception here if user logs out before closing wss connection
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;

            _data.ActiveNotificationUsers[userId]--;
            if(_data.ActiveNotificationUsers[userId] == 0)
            {
                _data.ActiveNotificationUsers.Remove(userId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task Read(int notificationId)
        {
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            var response = await _notificationService.ToggleTrueIsRead(notificationId);
            if(!response.IsError)
            {
                // If ToggleTrueIsRead method is succesful, inform other users that the notification has been read
                await Clients.Group($"push-{userId}").SendAsync("read-notification", notificationId);
            }
            else
            {   // If not, inform only the caller that an error occured.
                await Clients.Caller.SendAsync("error", response.StatusCode, response.Message);
            }
        }

        public async Task Ping()
        {
            pingCount++;
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            await Clients.Group($"push-{userId}").SendAsync("groupPing", userId, _data.ActiveNotificationUsers[userId], pingCount);
        }
    }
}
