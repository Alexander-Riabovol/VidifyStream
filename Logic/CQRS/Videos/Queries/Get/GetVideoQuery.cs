using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Videos.Queries.Get
{
    /// <summary>
    /// Retrieves a <see cref="Video"/> by its ID.
    /// </summary>
    public record GetVideoQuery(int VideoId) : IRequest<ServiceResponse<VideoGetDTO>>;
}
