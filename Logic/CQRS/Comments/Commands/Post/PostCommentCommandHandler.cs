using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Notifications.Commands.Push;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post
{
    public class PostCommentCommandHandler
        : IRequestHandler<PostCommentCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public PostCommentCommandHandler(DataContext dataContext,
                                         IHttpContextAccessor accessor,
                                         IMapper mapper,
                                         ISender mediator)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
            _mediator = mediator;
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
                var response = 
                await _mediator.Send(new PushNotificationCommand(new Notification()
                {
                    VideoId = request.CommentDto.VideoId,
                    CommentId = comment.CommentId,
                    UserId = video.UserId,
                    Type = NotificationType.LeftComment,
                    Message = $"New comment under your '{video.Title}' video: '{comment.Text}'.",
                    Date = DateTime.Now
                }));

                if (response.IsError) return new ServiceResponse<int>(response.StatusCode,
                                                                      response.Message!);
            }

            return ServiceResponse<int>.OK(comment.CommentId);
        }
    }
}
