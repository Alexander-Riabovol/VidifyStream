using Data.Context;
using Data.Dtos;
using Data.Dtos.Video;
using Data.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly DataContext _dataContext;

        public VideoService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<VideoGetDTO>> Get(int videoId)
        {
            var video = await _dataContext.Videos.Include(v => v.User)
                                                 .FirstOrDefaultAsync(v => v.VideoId == videoId);

            if(video == null) 
            {
                return new ServiceResponse<VideoGetDTO>(404, $"Video with ID {videoId} does not exist.");
            }

            var videoDto = video.Adapt<VideoGetDTO>();
            return ServiceResponse<VideoGetDTO>.OK(videoDto);
        }
    }
}
