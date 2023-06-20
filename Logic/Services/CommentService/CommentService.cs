using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CommentService(DataContext dataContext, IMapper mapper) 
        {
            _dataContext = dataContext;
            _mapper = mapper;
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
    }
}
