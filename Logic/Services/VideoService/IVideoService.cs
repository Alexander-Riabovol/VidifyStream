using Data.Dtos;
using Data.Dtos.Video;

namespace Logic.Services.VideoService
{
    public interface IVideoService
    {
        Task<ServiceResponse<VideoGetDTO>> Get(int videoId);
    }
}
