using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get.Replies
{
    public class GetRepliesQueryHandler :
        IRequestHandler<GetRepliesQuery, ServiceResponse<IEnumerable<ReplyGetDTO>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetRepliesQueryHandler(DataContext dataContext,
                                      IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<ReplyGetDTO>>> Handle(GetRepliesQuery request, CancellationToken cancellationToken)
        {
            var comment = await _dataContext.Comments.FindAsync(request.CommentId);

            if (comment == null)
            {
                return new ServiceResponse<IEnumerable<ReplyGetDTO>>(
                    404, $"Comment with ID {request.CommentId} does not exist.");
            }
            if (comment.RepliedToId != null)
            {
                return new ServiceResponse<IEnumerable<ReplyGetDTO>>(
                    400, $"A comment with ID {request.CommentId} that you have provided is is not directly posted beneath the video.");
            }

            var replies = await _dataContext.Comments
                                            .Where(c => c.RepliedToId == request.CommentId)
                                            .ToListAsync(cancellationToken);

            var repliesDtos = replies.Select(_mapper.Map<ReplyGetDTO>);

            return ServiceResponse<IEnumerable<ReplyGetDTO>>.OK(repliesDtos);
        }
    }
}
