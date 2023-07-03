using Data.Dtos.User;
using Data.Models;
using Mapster;

namespace Logic.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserRegisterDTO, User>()
                  // Remove all spaces inbetween words
                  .Map(dest => dest.Name, src => string.Join(' ',
                                               src.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                  // Omit time details
                  .Map(dest => dest.BirthDate, src => src.BirthDate.Date)
                  .Map(dest => dest.Status, src => Status.User);

            config.NewConfig<UserPutDTO, User>()
                  .IgnoreIf((src, dest) => src.Name == null, dest => dest.Name)
                  .IgnoreIf((src, dest) => src.BirthDate == null, dest => dest.BirthDate)
                  .IgnoreIf((src, dest) => src.Bio == null, dest => dest.Bio!);

            config.NewConfig<User, UserGetDTO>()
                  .Map(dest => dest.ProfilePictureUrl, src => src.ProfilePictureUrls.FirstOrDefault());

            config.NewConfig<User, UserAdminGetDTO>()
                  .Map(dest => dest.VideosIds, src => src.Videos!.Select(v => v.VideoId))
                  .Map(dest => dest.CommentsIds, src => src.Comments!.Select(c => c.CommentId))
                  .Map(dest => dest.NotificationsIds, src => src.Notifications!.Select(n => n.NotificationId));
        }
    }
}
