using Data.Context;
using Data.Dtos;
using Data.Dtos.Video;
using Data.Models;
using Logic.Extensions;
using Logic.Services.FileService;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _accessor;

        public VideoService(DataContext dataContext, IFileService fileService, IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _fileService = fileService;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<VideoGetDTO>> GetVideo(int videoId)
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

        public async Task<ServiceResponse> PostVideo(VideoPostDTO videoGetDTO)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var video = videoGetDTO.Adapt<Video>();

            var videoFileUploadResponse = await _fileService.Upload(videoGetDTO.VideoFile);
            if(videoFileUploadResponse.IsError) 
            { 
                return new ServiceResponse<VideoPostDTO>(videoFileUploadResponse.StatusCode,
                                                         videoFileUploadResponse.Message!);
            }

            var thumbnailFileUploadResponse = await _fileService.Upload(videoGetDTO.Thumbnail);
            if (thumbnailFileUploadResponse.IsError)
            {
                return new ServiceResponse<VideoPostDTO>(thumbnailFileUploadResponse.StatusCode,
                                                         thumbnailFileUploadResponse.Message!);
            }

            video.ThumbnailUrl = thumbnailFileUploadResponse.Content!;
            video.SourceUrl = videoFileUploadResponse.Content!;
            video.UserId = idResult.Content;

            await _dataContext.AddAsync(video);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }
    }
}
