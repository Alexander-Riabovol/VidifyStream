using Data.Context;
using Data.Dtos;
using Data.Dtos.Comment;
using Data.Models;
using MapsterMapper;

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

        public Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetCommentsByVideoId(int videoId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<IEnumerable<CommentGetDTO>>> GetReplies(int commentId)
        {
            var comment = await _dataContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return new ServiceResponse<IEnumerable<CommentGetDTO>>(
                    404, $"Comment with ID {commentId} does not exist.");
            }
            if (comment.RepliedToId != null)
            {
                return new ServiceResponse<IEnumerable<CommentGetDTO>>(
                    400, $"A comment with ID {commentId}  that you have provided is is not directly posted beneath the video.");
            }

            throw new NotImplementedException();
        }
    }
}
