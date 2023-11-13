using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Delete
{
    public class DeleteVideoCommandHandler
        : IRequestHandler<DeleteVideoCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<DeleteVideoCommandHandler> _logger;


        public DeleteVideoCommandHandler(DataContext dataContext,
                            IHttpContextAccessor accessor,
                            ILogger<DeleteVideoCommandHandler> logger)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
        }

        public async Task<ServiceResponse> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            Data.Models.Video? video;
            
            if(request.Admin)
            {
                video = await _dataContext.Videos
                                          .IgnoreQueryFilters()
                                          .FirstOrDefaultAsync(v => v.VideoId == request.VideoId,
                                                               cancellationToken);
            }
            else { video = await _dataContext.Videos.FindAsync(request.VideoId); }

            if (video == null)
            {
                return new ServiceResponse(404,
                    request.Admin 
                    ? $"Video with ID {request.VideoId} was not found in the database."
                    : $"Video with ID already {request.VideoId} does not exist.");
            }

            if(request.Admin && video.DeletedAt != null)
            {
                return ServiceResponse.NotModified;
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);


            if (request.Admin)
            {
                var admin = await _dataContext.Users.FindAsync(idResult.Content);
                _logger.LogInformation($"Admin [Name: {admin?.Name}, ID: {{Admin?.UserId}}] deleted Video [Title: {video.Title}, ID: {{Video.VideoId}}].", admin, video);
            }
            else if (video.UserId != idResult.Content)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            _dataContext.Remove(video);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
