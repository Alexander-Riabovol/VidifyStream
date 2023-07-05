using Data.Dtos.Video;
using Data.Models;
using Mapster;

namespace Logic.Mapping
{
    /// <summary>
    /// Configuration class for mapping VideoDTOs to <see cref="Video"/> models and vice versa.
    /// </summary>
    internal class VideoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<VideoPutDTO, Video>()
                  .IgnoreIf((src, dest) => src.Title == null, dest => dest.Title)
                  .IgnoreIf((src, dest) => src.Description == null, dest => dest.Description)
                  .IgnoreIf((src, dest) => src.Category == null, dest => dest.Category)
                  .IgnoreIf((src, dest) => src.Thumbnail == null, dest => dest.ThumbnailUrl);

            config.NewConfig<Video, VideoGetDTO>()
                  .IgnoreIf((dest, src) => dest.User == null, dest => dest.UserName, dest => dest.UserProfilePictureUrl)
                  .Map(dest => dest.UserName, src => src.User!.Name)
                  .Map(dest => dest.UserProfilePictureUrl, src => src.User!.ProfilePictureUrls.FirstOrDefault());
        }
    }
}
