using MediatR;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.CQRS.Video.Queries.Get
{
    public record GetVideoQuery(int VideoId) : IRequest<ServiceResponse<VideoGetDTO>>;
}
