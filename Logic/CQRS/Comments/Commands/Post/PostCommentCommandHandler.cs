using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Notifications;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post
{
    public class PostCommentCommandHandler
        : IRequestHandler<PostCommentCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public PostCommentCommandHandler(DataContext dataContext,
                                         IHttpContextAccessor accessor,
                                         IMapper mapper,
                                         INotificationService notificationService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ServiceResponse<int>> Handle(PostCommentCommand request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<int>(idResult.StatusCode, idResult.Message!);

            var comment = _mapper.Map<Comment>(request.CommentDto);

            comment.UserId = idResult.Content;
            await _dataContext.AddAsync(comment, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            var video = (await _dataContext.Videos.FindAsync(request.CommentDto.VideoId))!;
            // If a user left a comment under his own video, do not send him a notification.
            if (video.UserId != comment.UserId)
            {
                await _notificationService.CreateAndSend(new Notification()
                {
                    VideoId = request.CommentDto.VideoId,
                    CommentId = comment.CommentId,
                    UserId = video.UserId,
                    Type = NotificationType.LeftComment,
                    Message = $"New comment under your '{video.Title}' video: '{comment.Text}'.",
                    Date = DateTime.Now
                });
            }

            return ServiceResponse<int>.OK(comment.CommentId);
        }
    }
}
