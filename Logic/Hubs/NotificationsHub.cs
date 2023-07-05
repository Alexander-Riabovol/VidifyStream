﻿using Logic.Services.AuthService;
using Logic.Services.NotificationService;
using Microsoft.AspNetCore.SignalR;

namespace Logic.Hubs
{
    // Hub sent events:
    // {"type": 1,"target": "broadcast-notifications","arguments": []} - sent whenever there is a new notification, in OnConnectedAsync() and in GetAll().
    // {"type": 1,"target": "error","arguments": []} - sent as a response to failed client command execution.
    // {"type":1,"target":"read-notification","arguments":[notificationId]} - sent to the group as a sign that a notifications has been read.
    // {"type":1,"target":"delete-notification","arguments":[notificationId]} - sent to the group as a sign that a notifications has been deleted.
    // {"type":1,"target":"group-ping","arguments":[userId, numberOfActiveConnectionsInTheGroup, pingCount]} - pretty self explanatory.
    // Client commands:
    // {"protocol":"json","version":1} - start communication
    // {"arguments":[],"target":"ping","type":1} - ping function for testing
    // {"arguments":[],"target":"getall","type":1} - get all notifications
    // {"arguments":[notificationId],"target":"read","type":1} - read a notification by id
    // {"arguments":[notificationId],"target":"delete","type":1} - delete a notification by id
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
                var response = await _notificationService.GetAllMy();
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
            // Context.Abort() in OnConnectedAsync function calls OnDisconnectedAsync()
            // in any case, so an identity check will prevent InvalidOperationException
            var identity = Context.User?.Identity;
            if (identity == null || !identity.IsAuthenticated
                || identity.AuthenticationType != IAuthService.AuthScheme)
            {
                return base.OnDisconnectedAsync(exception);
            }

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

        public async Task Delete(int notificationId)
        {
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            var response = await _notificationService.Delete(notificationId);
            if (!response.IsError)
            {
                // If Delete method is succesful, inform other users that the notification has been deleted
                await Clients.Group($"push-{userId}").SendAsync("delete-notification", notificationId);
            }
            else
            {   // If not, inform only the caller that an error occured.
                await Clients.Caller.SendAsync("error", response.StatusCode, response.Message);
            }
        }

        public async Task GetAll()
        {
            var response = await _notificationService.GetAllMy(false);
            // In any case we give a response only to the caller
            if (!response.IsError)
            {
                await Clients.Caller.SendAsync("broadcast-notifications", response.Content);
            }
            else
            {
                await Clients.Caller.SendAsync("error", response.StatusCode, response.Message);
            }
        }

        public async Task Ping()
        {
            pingCount++;
            string userId = Context.User!.Claims.First(c => c.Type == "id")!.Value;
            await Clients.Group($"push-{userId}").SendAsync("group-ping", userId, _data.ActiveNotificationUsers[userId], pingCount);
        }
    }
}
