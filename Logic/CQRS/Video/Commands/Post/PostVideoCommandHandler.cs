using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Files;

namespace VidifyStream.Logic.CQRS.Video.Commands.Post
{
    public class PostVideoCommandHandler :
        IRequestHandler<PostVideoCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public PostVideoCommandHandler(DataContext dataContext,
                                       IHttpContextAccessor accessor,
                                       IMapper mapper,
                                       IFileService fileService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<int>> Handle(PostVideoCommand request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<int>(idResult.StatusCode, idResult.Message!);
            var video = _mapper.Map<Data.Models.Video>(request.Video);

            var videoFileUploadResponse = await _fileService.Upload(request.Video.VideoFile);
            if (videoFileUploadResponse.IsError)
            {
                return new ServiceResponse<int>(videoFileUploadResponse.StatusCode,
                videoFileUploadResponse.Message!);
            }

            var thumbnailFileUploadResponse = await _fileService.Upload(request.Video.Thumbnail);
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
    }
}
