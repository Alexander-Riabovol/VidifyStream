﻿using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.Notification;
using Data.Models;
using Logic.Extensions;
using Logic.Services.NotificationService;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Logic.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _accessor;

        public CommentService(DataContext dataContext,
                              IMapper mapper,
                              INotificationService notificationService,
                              IHttpContextAccessor accessor) 
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _notificationService = notificationService;
            _accessor = accessor;
        }

        public async Task<ServiceResponse<CommentGetDTO>> GetComment(int commentId)
        {
            var comment = await _dataContext.Comments.FindAsync(commentId);

            if(comment == null)
            {
                return new ServiceResponse<CommentGetDTO>(404, $"Comment with ID {commentId} does not exist.");
            }

            var commentDto = _mapper.Map<CommentGetDTO>(comment);
            return ServiceResponse<CommentGetDTO>.OK(commentDto);
        }

        public async Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId)
        {
            var video = await _dataContext.Videos.Include(v => v.Comments)
                                                 .FirstOrDefaultAsync(v => v.VideoId == videoId);

            if (video == null)
            {
                return new ServiceResponse<IEnumerable<CommentGetDTO>>(
                    404, $"Video with ID {videoId} does not exist.");
            }

            var commentsDtos = video.Comments?.Select(_mapper.Map<CommentGetDTO>);
            return ServiceResponse<IEnumerable<CommentGetDTO>>.OK(commentsDtos);
        }

        public async Task<ServiceResponse<IEnumerable<ReplyGetDTO>>> GetReplies(int commentId)
        {
            var comment = await _dataContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return new ServiceResponse<IEnumerable<ReplyGetDTO>>(
                    404, $"Comment with ID {commentId} does not exist.");
            }
            if (comment.RepliedToId != null)
            {
                return new ServiceResponse<IEnumerable<ReplyGetDTO>>(
                    400, $"A comment with ID {commentId} that you have provided is is not directly posted beneath the video.");
            }

            var replies = await _dataContext.Comments.Where(c => c.RepliedToId == commentId).ToListAsync();
            var repliesDtos = replies.Select(_mapper.Map<ReplyGetDTO>);

            return ServiceResponse<IEnumerable<ReplyGetDTO>>.OK(repliesDtos);
        }

        public async Task<ServiceResponse<int>> PostComment(CommentPostDTO commentPostDTO)
        {
            var comment = _mapper.Map<Comment>(commentPostDTO);
            comment.UserId = int.Parse(_accessor.HttpContext!.User!.Claims.First(c => c.Type == "id")!.Value);

            await _dataContext.AddAsync(comment);
            await _dataContext.SaveChangesAsync();

            var video = (await _dataContext.Videos.FindAsync(commentPostDTO.VideoId))!;
            await _notificationService.CreateAndSend(new NotificationCreateDTO()
            { 
                VideoId = commentPostDTO.VideoId,
                CommentId = comment.CommentId,
                UserId = video.UserId,
                Type = NotificationType.LeftComment,
                Message = $"New comment under your '{video.Title}' video: '{comment.Text}'."
            });

            return ServiceResponse<int>.OK(comment.CommentId);
        }

        public async Task<ServiceResponse<int>> PostReply(ReplyPostDTO replyPostDTO)
        {
            var comment = _mapper.Map<Comment>(replyPostDTO);

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<int>(idResult.StatusCode, idResult.Message!);

            comment.UserId = idResult.Content;

            await _dataContext.AddAsync(comment);
            await _dataContext.SaveChangesAsync();

            var repliedTo = (await _dataContext.Comments.FindAsync(replyPostDTO.RepliedToId))!;
            var top = repliedTo;
            while(top!.RepliedToId != null)
            {
                top = await _dataContext.Comments.FindAsync(top!.RepliedToId);
            }

            var user = await _dataContext.Users.FindAsync(comment.UserId);

            await _notificationService.CreateAndSend(new NotificationCreateDTO()
            {
                VideoId = top!.VideoId,
                CommentId = comment.CommentId,
                UserId = repliedTo.UserId,
                Type = NotificationType.Reply,
                Message = $"{user!.Name} replied to your comment: '{comment.Text}'."
            });

            return ServiceResponse<int>.OK(comment.CommentId);
        }

        public async Task<ServiceResponse> Put(CommentPutDTO commentPutDTO)
        {
            var comment = await _dataContext.Comments.FindAsync(commentPutDTO.CommentId);

            if(comment == null)
            {
                return new ServiceResponse(404, $"Comment with ID {commentPutDTO.CommentId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if(idResult.Content != comment.UserId)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            comment = _mapper.Map(commentPutDTO, comment);

            _dataContext.Update(comment);
            await _dataContext.SaveChangesAsync();

            return ServiceResponse.OK;
        }
    }
}
