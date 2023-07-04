using Data.Dtos;
using Data.Dtos.Video;

namespace Logic.Services.VideoService
{
    public interface IVideoService
    {
        Task<ServiceResponse<VideoGetDTO>> GetVideo(int videoId);
        Task<ServiceResponse<int>> PostVideo(VideoPostDTO videoPostDTO);
        Task<ServiceResponse> PutVideo(VideoPutDTO videoPutDTO);
        Task<ServiceResponse> Delete(int videoId);
        Task<ServiceResponse> DeleteAdmin(int videoId);
    }
}
