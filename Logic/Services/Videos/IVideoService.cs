using VidifyStream.Data.Models;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;

namespace VidifyStream.Logic.Services.Videos
{
    /// <summary>
    /// Represents a service for managing videos.
    /// </summary>
    public interface IVideoService
    {
        /// <summary>
        /// Retrieves a <see cref="Video"/> by its ID.
        /// </summary>
        Task<ServiceResponse<VideoGetDTO>> GetVideo(int videoId);
        /// <summary>
        /// Creates a new <see cref="Video"/>.
        /// </summary>
        Task<ServiceResponse<int>> PostVideo(VideoPostDTO videoPostDTO);
        /// <summary>
        /// Updates a <see cref="Video"/>'s information.
        /// </summary>
        Task<ServiceResponse> PutVideo(VideoPutDTO videoPutDTO);
        /// <summary>
        /// Deletes a <see cref="Video"/> by its ID, if it's the authenticated <see cref="User"/>'s video.
        /// </summary>
        Task<ServiceResponse> Delete(int videoId);
        /// <summary>
        /// Deletes any <see cref="Video"/> by its ID.
        /// </summary>
        Task<ServiceResponse> DeleteAdmin(int videoId);
    }
}
