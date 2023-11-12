using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Notifications.Commands.Push;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Notifications;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Put.Like
{
    public class ToggleLikeCommandHandler :
        IRequestHandler<ToggleLikeCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISender _mediator;

        public ToggleLikeCommandHandler(DataContext dataContext,
                                        IHttpContextAccessor accessor,
                                        ISender mediator)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mediator = mediator;
        }

        public async Task<ServiceResponse> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
        {
            var comment = await _dataContext.Comments.FindAsync(request.CommandId);

            if (comment == null)
            {
                return new ServiceResponse(404, $"Comment with ID {request.CommandId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var topComment = comment;
            while (topComment!.VideoId == null)
            {
                topComment = await _dataContext.Comments.FindAsync(topComment.RepliedToId);
            }

            var video = await _dataContext.Videos.FindAsync(topComment.VideoId);

            if (video != null && video.UserId == idResult.Content)
            {
                // There is currently an abuse of this endpoint: you can send endless amount of notifications to
                // the user as an author, but it will be fixed once I implement storing likes in the database.
                comment.IsAuthorLiked = !comment.IsAuthorLiked;
                // If an author left a like under his own comment, or if the author
                // removed his like: do not send a notification.
                if (comment.IsAuthorLiked && comment.UserId != idResult.Content)
                {
                    var author = await _dataContext.Users.FindAsync(idResult.Content);
                    var response =
                    await _mediator.Send(new PushNotificationCommand(new Notification()
                    {
                        VideoId = topComment.VideoId,
                        CommentId = comment.CommentId,
                        UserId = comment.UserId,
                        Type = NotificationType.AuthorLikedComment,
                        Message = $"{author?.Name} liked your comment: '{comment.Text}'.",
                        Date = DateTime.Now
                    }));

                    if (response.IsError) return new ServiceResponse<int>(response.StatusCode,
                                                                          response.Message!);
                }
            }
            else
            {   // Add Logic if user is not the author of the video (regular like)
                return new ServiceResponse(500, "Not Implemented. Sorry, but like endpoint works for authors of videos only for now!");
            }

            _dataContext.Update(comment);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
