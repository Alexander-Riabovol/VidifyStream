using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Files;

namespace VidifyStream.Logic.CQRS.Video.Commands.Put
{
    public class PutVideoCommandHandler :
        IRequestHandler<PutVideoCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public PutVideoCommandHandler(DataContext dataContext,
                                      IHttpContextAccessor accessor,
                                      IMapper mapper,
                                      IFileService fileService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(PutVideoCommand request, CancellationToken cancellationToken)
        {
            var video = await _dataContext.Videos.FindAsync(request.Video.VideoId);

            if (video == null)
            {
                return new ServiceResponse(404, $"Video with ID {request.Video.VideoId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if (idResult.Content != video.UserId)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            video = _mapper.Map(request.Video, video);

            if (request.Video.Thumbnail != null)
            {
                var thumbnailFileUploadResponse = await _fileService.Upload(request.Video.Thumbnail);
                if (thumbnailFileUploadResponse.IsError)
                {
                    return new ServiceResponse<int>(thumbnailFileUploadResponse.StatusCode,
                                                    thumbnailFileUploadResponse.Message!);
                }
                video.ThumbnailUrl = thumbnailFileUploadResponse.Content!;
            }

            _dataContext.Update(video);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}