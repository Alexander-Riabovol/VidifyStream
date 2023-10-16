using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;

namespace VidifyStream.Logic.CQRS.Videos.Queries.Get
{
    public class GetVideoQueryHandler :
        IRequestHandler<GetVideoQuery, ServiceResponse<VideoGetDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetVideoQueryHandler(DataContext dataContext,
                                    IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<VideoGetDTO>> Handle(GetVideoQuery request, CancellationToken cancellationToken)
        {
            var video = await _dataContext.Videos.Include(v => v.User)
                                                 .FirstOrDefaultAsync(v => v.VideoId == request.VideoId, cancellationToken);

            if (video == null)
            {
                return new ServiceResponse<VideoGetDTO>(404, $"Video with ID {request.VideoId} does not exist.");
            }

            var videoDto = _mapper.Map<VideoGetDTO>(video);
            return ServiceResponse<VideoGetDTO>.OK(videoDto);
        }
    }
}