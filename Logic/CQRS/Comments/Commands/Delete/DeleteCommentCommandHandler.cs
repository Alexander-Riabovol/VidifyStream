using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Delete
{
    public class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;

        public DeleteCommentCommandHandler(DataContext dataContext,
                                           IHttpContextAccessor accessor,
                                           ILogger<DeleteCommentCommandHandler> logger)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _logger = logger;
        }

        public async Task<ServiceResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            Comment? comment;
            
            if (request.Admin)
            {
                comment = await _dataContext.Comments
                                            .IgnoreQueryFilters()
                                            .FirstOrDefaultAsync(c => c.CommentId == request.CommentId,
                                                                 cancellationToken);
            }
            else
            {
                comment = await _dataContext.Comments.FindAsync(request.CommentId);
            }
            

            if (comment == null)
            {
                return new ServiceResponse(
                    404, 
                    request.Admin ? $"Comment with ID {request.CommentId} was not found in the database."
                                  : $"Comment with ID already {request.CommentId} does not exist.");
            }

            if (comment.DeletedAt != null)
            {
                return ServiceResponse.NotModified;
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if (request.Admin)
            {
                var admin = await _dataContext.Users.FindAsync(idResult.Content);
                _logger.LogInformation($"Admin {{Name: {admin?.Name}, ID: {admin?.UserId}}} deleted Comment {{ID: {comment.CommentId}}}.");
            }
            else if (comment.UserId != idResult.Content)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            _dataContext.Remove(comment);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
