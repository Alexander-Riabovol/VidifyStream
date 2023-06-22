using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.Notification;
using Data.Models;
using Logic.Services.NotificationService;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
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

        public async Task<ServiceResponse<IEnumerable<CommentReplyGetDTO>>> GetReplies(int commentId)
        {
            var comment = await _dataContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return new ServiceResponse<IEnumerable<CommentReplyGetDTO>>(
                    404, $"Comment with ID {commentId} does not exist.");
            }
            if (comment.RepliedToId != null)
            {
                return new ServiceResponse<IEnumerable<CommentReplyGetDTO>>(
                    400, $"A comment with ID {commentId} that you have provided is is not directly posted beneath the video.");
            }

            var replies = await _dataContext.Comments.Where(c => c.RepliedToId == commentId).ToListAsync();
            var repliesDtos = replies.Select(_mapper.Map<CommentReplyGetDTO>);

            return ServiceResponse<IEnumerable<CommentReplyGetDTO>>.OK(repliesDtos);
        }

        public async Task<ServiceResponse> PostComment(int videoId, CommentPostDTO commentDto)
        {
            // MOVE VALIDATION SOMEWHERE
            if (commentDto == null || commentDto.Text == null)
            {
                return new ServiceResponse(400, "Bad Request");
            }

            var video = await _dataContext.Videos.FindAsync(videoId);
            if(video == null)
            {
                return new ServiceResponse(404, $"Video with ID {videoId} does not exist.");
            }

            var comment = _mapper.Map<Comment>(commentDto);
            comment.VideoId = videoId;
            comment.UserId = int.Parse(_accessor.HttpContext!.User!.Claims.First(c => c.Type == "id")!.Value);

            await _dataContext.AddAsync(comment);
            await _dataContext.SaveChangesAsync();

            await _notificationService.CreateAndSend(new NotificationCreateDTO()
            { 
                VideoId = videoId,
                CommentId = comment.CommentId,
                UserId = video.UserId,
                Type = NotificationType.LeftComment,
                Message = $"New comment under your '{video.Title}' video: '{comment.Text}'."
            });

            return ServiceResponse.OK;
        }

        public async Task<ServiceResponse> PostReply(int repliedToId, CommentPostDTO commentDto)
        {
            // MOVE VALIDATION SOMEWHERE
            if (commentDto == null || commentDto.Text == null)
            {
                return new ServiceResponse(400, "Bad Request");
            }

            var repliedTo = await _dataContext.Comments.FindAsync(repliedToId);
            if (repliedTo == null)
            {
                return new ServiceResponse(404, $"Comment with ID {repliedToId} does not exist.");
            }

            var comment = _mapper.Map<Comment>(commentDto);
            comment.RepliedToId = repliedToId;
            comment.UserId = int.Parse(_accessor.HttpContext!.User!.Claims.First(c => c.Type == "id")!.Value);

            await _dataContext.AddAsync(comment);
            await _dataContext.SaveChangesAsync();

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

            return ServiceResponse.OK;
        }
    }
}
