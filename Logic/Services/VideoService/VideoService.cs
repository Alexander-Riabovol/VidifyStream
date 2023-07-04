using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.Video;
using Data.Models;
using Logic.Extensions;
using Logic.Services.FileService;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.VideoService
{
    public class VideoService : IVideoService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _accessor;

        public VideoService(DataContext dataContext,
                            IMapper mapper, 
                            IFileService fileService,
                            IHttpContextAccessor accessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
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
    }
}
