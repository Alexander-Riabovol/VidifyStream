using Data.Dtos.Notification;
using Data.Models;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Data.Mapping
{
    public class NotificationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NotificationCreateDTO, Notification>()
                  .Map(dest => dest.Date, src => DateTime.Now);

            config.NewConfig<Notification, NotificationGetDTO>()
                  .Map(dest => dest.ActionLink, src => GetActionLink(src));
        }

        // If we had a Frontend UI, our approach to composing the ActionLink would be different.
        // The purpose of the ActionLink is to redirect the user to the page where they can view
        // the event that triggered the notification.
        // For example, on YouTube, if you click on a notification that says
        // "There's a new video in your subscriptions", you are redirected to the video itself.
        // If we apply the same logic but without a Frontend, the best we can do is provide
        // a user with a link to the API that will return the information about the new video.
        private string? GetActionLink(Notification src)
        {
            switch(src.Type)
            {
                case NotificationType.SubscribersGoal:
                    return $"/api/users/{src.UserId}";
                case NotificationType.Reply:
                case NotificationType.AuthorLikedComment:
                case NotificationType.LeftComment:
                    return $"/api/comments/{src.CommentId}";
                case NotificationType.NewSubscribtionsVideo:
                case NotificationType.RecommendedVideo:
                    return $"/api/video/{src.VideoId}";
                default: 
                    return null;
            }
        }

        //private string? GetUserThumbnail(Notification src) 
        //{
            
        //}
    }
}
