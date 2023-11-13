using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get.Video
{
    public class GetCommentsByVideoQueryHandler :
        IRequestHandler<GetCommentsByVideoQuery, ServiceResponse<IEnumerable<CommentGetDTO>>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetCommentsByVideoQueryHandler(DataContext dataContext,
                                              IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<CommentGetDTO>>> Handle(GetCommentsByVideoQuery request, CancellationToken cancellationToken)
        {
            var video = await _dataContext.Videos.Include(v => v.Comments)
                                                 .FirstOrDefaultAsync(v => v.VideoId == request.VideoId,
                                                                      cancellationToken);

            if (video == null)
            {
                return new ServiceResponse<IEnumerable<CommentGetDTO>>(
                    404, $"Video with ID {request.VideoId} does not exist.");
            }

            var commentsDtos = video.Comments?.Select(_mapper.Map<CommentGetDTO>);
            return ServiceResponse<IEnumerable<CommentGetDTO>>.OK(commentsDtos);
        }
    }
}
