﻿using Data.Context;
using Data.Dtos;
using Data.Dtos.Video;
using Data.Models;
using Logic.Extensions;
using Logic.Services.Files;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Services.Videos
{
    public class VideoService : IVideoService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<VideoService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public VideoService(DataContext dataContext,
                            IHttpContextAccessor accessor,
                            ILogger<VideoService> logger,
                            IMapper mapper,
                            IFileService fileService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
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

        public async Task<ServiceResponse<int>> PostVideo(VideoPostDTO videoPostDTO)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<int>(idResult.StatusCode, idResult.Message!);

            var video = videoPostDTO.Adapt<Video>();

            var videoFileUploadResponse = await _fileService.Upload(videoPostDTO.VideoFile);
            if(videoFileUploadResponse.IsError) 
            { 
                return new ServiceResponse<int>(videoFileUploadResponse.StatusCode,
                                                videoFileUploadResponse.Message!);
            }

            var thumbnailFileUploadResponse = await _fileService.Upload(videoPostDTO.Thumbnail);
            if (thumbnailFileUploadResponse.IsError)
            {
                return new ServiceResponse<int>(thumbnailFileUploadResponse.StatusCode,
                                                thumbnailFileUploadResponse.Message!);
            }

            video.ThumbnailUrl = thumbnailFileUploadResponse.Content!;
            video.SourceUrl = videoFileUploadResponse.Content!;
            video.UserId = idResult.Content;

            await _dataContext.AddAsync(video);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse<int>.OK(video.VideoId);
        }

        public async Task<ServiceResponse> PutVideo(VideoPutDTO videoPutDTO)
        {
            var video = await _dataContext.Videos.FindAsync(videoPutDTO.VideoId);

            if (video == null)
            {
                return new ServiceResponse(404, $"Video with ID {videoPutDTO.VideoId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if (idResult.Content != video.UserId)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            video = _mapper.Map(videoPutDTO, video);

            if (videoPutDTO.Thumbnail != null)
            {
                var thumbnailFileUploadResponse = await _fileService.Upload(videoPutDTO.Thumbnail);
                if (thumbnailFileUploadResponse.IsError)
                {
                    return new ServiceResponse<int>(thumbnailFileUploadResponse.StatusCode,
                                                    thumbnailFileUploadResponse.Message!);
                }
                video.ThumbnailUrl = thumbnailFileUploadResponse.Content!;
            }

            _dataContext.Update(video);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> Delete(int videoId)
        {
            var video = await _dataContext.Videos.FindAsync(videoId);

            if (video == null)
            {
                return new ServiceResponse(404, $"Video with ID already {videoId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if (video.UserId != idResult.Content)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            _dataContext.Remove(video);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> DeleteAdmin(int videoId)
        {
            var video = await _dataContext.Videos.IgnoreQueryFilters()
                                                 .FirstOrDefaultAsync(v => v.VideoId == videoId);

            if (video == null)
            {
                return new ServiceResponse(404, $"Video with ID {videoId} was not found in the database.");
            }
            if (video.DeletedAt != null)
            {
                return ServiceResponse.NotModified;
            }

            _dataContext.Remove(video);
            await _dataContext.SaveChangesAsync();

            var idResult = _accessor.HttpContext!.RetriveUserId();
            var admin = await _dataContext.Users.FindAsync(idResult.Content);
            _logger.LogInformation($"Admin {{Name: {admin?.Name}, ID: {admin?.UserId}}} deleted Video {{Title: {video.Title}, ID: {{videoId}}}}.", video.VideoId);

            return ServiceResponse.OK;
        }
    }
}