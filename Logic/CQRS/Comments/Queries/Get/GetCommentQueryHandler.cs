using MapsterMapper;
using MediatR;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get
{
    public class GetCommentQueryHandler
        : IRequestHandler<GetCommentQuery, ServiceResponse<CommentGetDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        
        public GetCommentQueryHandler(DataContext dataContext,
                                      IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CommentGetDTO>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            var comment = await _dataContext.Comments.FindAsync(request.CommentId);

            if (comment == null)
            {
                return new ServiceResponse<CommentGetDTO>(
                    404, $"Comment with ID {request.CommentId} does not exist.");
            }

            var commentDto = _mapper.Map<CommentGetDTO>(comment);
            return ServiceResponse<CommentGetDTO>.OK(commentDto);
        }
    }
}
