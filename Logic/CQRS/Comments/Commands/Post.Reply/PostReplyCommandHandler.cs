﻿using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Notifications.Commands.Push;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post.Reply
{
    public class PostReplyCommandHandler
        : IRequestHandler<PostReplyCommand, ServiceResponse<int>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public PostReplyCommandHandler(DataContext dataContext,
                                       IHttpContextAccessor accessor,
                                       IMapper mapper,
                                       ISender mediator)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ServiceResponse<int>> Handle(PostReplyCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(request.ReplyDto);

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<int>(idResult.StatusCode, idResult.Message!);

            comment.UserId = idResult.Content;
            await _dataContext.AddAsync(comment);
            await _dataContext.SaveChangesAsync(cancellationToken);

            var repliedTo = (await _dataContext.Comments.FindAsync(request.ReplyDto.RepliedToId))!;
            var top = repliedTo;
            while (top!.RepliedToId != null)
            {
                top = await _dataContext.Comments.FindAsync(top!.RepliedToId);
            }

            var user = await _dataContext.Users.FindAsync(comment.UserId);
            // If a user replied to his own comment, do not send him a notification.
            if (repliedTo.UserId != comment.UserId)
            {
                var response = 
                await _mediator.Send(new PushNotificationCommand(new Notification()
                {
                    VideoId = top!.VideoId,
                    CommentId = comment.CommentId,
                    UserId = repliedTo.UserId,
                    Type = NotificationType.Reply,
                    Message = $"{user!.Name} replied to your comment: '{comment.Text}'.",
                    Date = DateTime.Now
                }));

                if(response.IsError) return new ServiceResponse<int>(response.StatusCode,
                                                                     response.Message!);
            }

            return ServiceResponse<int>.OK(comment.CommentId);
        }
    }
}
