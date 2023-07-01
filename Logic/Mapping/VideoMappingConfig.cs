using Data.Context;
using Data.Dtos.Video;
using Data.Models;
using Mapster;

namespace Logic.Mapping
{
    public class VideoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Video, VideoGetDTO>()
                  .IgnoreIf((dest, src) => dest.User == null, dest => dest.UserName, dest => dest.UserProfilePictureUrl)
                  .Map(dest => dest.UserName, src => src.User!.Name)
                  .Map(dest => dest.UserProfilePictureUrl, src => src.User!.ProfilePictureUrls.FirstOrDefault());
        }
    }
}
