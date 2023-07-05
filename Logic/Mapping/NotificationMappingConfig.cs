using Data.Context;
using Data.Dtos.Notification;
using Data.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Logic.Mapping
{
    /// <summary>
    /// Configuration class for mapping NotificationDTOs to <see cref="Notification"/> models and vice versa.
    /// </summary>
    internal class NotificationMappingConfig : IRegister
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;

#pragma warning disable CS8618 // We need a parameterless .ctor for the config.Scan() method to work.
        public NotificationMappingConfig() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public NotificationMappingConfig(DataContext dataContext, IHttpContextAccessor accessor) 
        {
            _dataContext = dataContext;
            _accessor = accessor;
        }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NotificationAdminCreateDTO, NotificationCreateDTO>()
                  .Map(dest => dest.Type, serc => NotificationType.Blank);

            config.NewConfig<NotificationCreateDTO, Notification>()
                  .Map(dest => dest.Date, src => DateTime.Now);

            config.NewConfig<Notification, NotificationGetDTO>()
                  .Map(dest => dest.ActionLink, src => GetActionLink(src))
                  .AfterMappingAsync(RetrieveUrls);
        }

        // If we had a Frontend UI, our approach to composing the ActionLink would be different.
        // The purpose of the ActionLink is to redirect the user to the page where they can view
        // the event that triggered the notification.
        // For example, on YouTube, if you click on a notification that says
        // "There's a new video in your subscriptions", you are redirected to the video itself.
        // If we apply the same logic but without a Frontend, the best we can do is provide
        // a user with a link to the API that will return the information about the new video.
        /// <summary>
        /// Gets the action link based on the <see cref="NotificationType"/> and constructs the URL.
        /// </summary>
        /// <returns>The action link URL that points to the corresponding resource.</returns>
        private string? GetActionLink(Notification src)
        {
            var scheme = _accessor.HttpContext!.Request.Scheme;
            var host = _accessor.HttpContext!.Request.Host.ToUriComponent();
            switch (src.Type)
            {
                case NotificationType.SubscribersGoal:
                    return $"{scheme}://{host}/api/users/{src.UserId}";
                case NotificationType.Reply:
                case NotificationType.AuthorLikedComment:
                case NotificationType.LeftComment:
                    return $"{scheme}://{host}/api/comments/{src.CommentId}";
                case NotificationType.NewSubscribtionsVideo:
                case NotificationType.RecommendedVideo:
                    return $"{scheme}://{host}/api/videos/{src.VideoId}";
                default: 
                    return null;
            }
        }
        /// <summary>
        /// Retrieves additional URLs and sets them in the destination NotificationGetDTO instance.
        /// </summary>
        private async Task RetrieveUrls(Notification src, NotificationGetDTO dest) 
        {
            switch (src.Type)
            {
                case NotificationType.SubscribersGoal:
                    var user = await _dataContext.Users.FindAsync(src.UserId);
                    // To do: change the incoming pfp url to subscriber's one
                    dest.UserProfilePictureUrl = user?.ProfilePictureUrls?.FirstOrDefault()!;
                    break;
                // If it is a notification about a reply to your comment or a new comment under your video:
                // Set the notification video thumbnail to that of the video under which the comment is left, and
                // Set the notification user's profile picture to the one that whoever left the comment has.
                case NotificationType.Reply:
                    var reply = await _dataContext.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.CommentId == src.CommentId);
                    dest.VideoThumbnailUrl = (await _dataContext.Videos.FindAsync(src.VideoId))?.ThumbnailUrl!;
                    dest.UserProfilePictureUrl = reply?.User?.ProfilePictureUrls?.FirstOrDefault()!;
                    break;
                case NotificationType.LeftComment:
                    var leftcomment = await _dataContext.Comments.Include(c => c.Video).Include(c => c.User).FirstOrDefaultAsync(c => c.CommentId == src.CommentId);
                    dest.VideoThumbnailUrl = leftcomment?.Video?.ThumbnailUrl!;
                    dest.UserProfilePictureUrl = leftcomment?.User?.ProfilePictureUrls?.FirstOrDefault()!;
                    break;
                // If it is a notification about an author leaving a like under a comment:
                // Set the notification video thumbnail to that of the video under which the comment is left, and
                // Set the notification user's profile picture to the one that the author of the video has.
                case NotificationType.AuthorLikedComment:
                    var comment = await _dataContext.Comments.Include(c => c.Video).FirstOrDefaultAsync(c => c.CommentId == src.CommentId);
                    if(comment != null && comment.Video != null)
                    {
                        dest.VideoThumbnailUrl = comment.Video.ThumbnailUrl;
                        var author = await _dataContext.Users.FindAsync(comment.Video.UserId);
                        dest.UserProfilePictureUrl = author?.ProfilePictureUrls?.FirstOrDefault()!;
                    }
                    break;
                // If it is a notification about a new/recommended video:
                // Set the notification video thumbnail to that of a new video,
                // Set the notification user pfp to the one which the author of the video currently has.
                case NotificationType.NewSubscribtionsVideo:
                case NotificationType.RecommendedVideo:
                    var video = await _dataContext.Videos.Include(v => v.User).FirstOrDefaultAsync(v => v.VideoId == src.VideoId);
                    if (video != null)
                    {
                        dest.VideoThumbnailUrl = video.ThumbnailUrl;
                        dest.UserProfilePictureUrl = video.User?.ProfilePictureUrls?.FirstOrDefault()!;
                    }
                    break;
            }
        }
    }
}
