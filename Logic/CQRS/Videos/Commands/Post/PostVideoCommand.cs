using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Post
{
    /// <summary>
    /// Creates a new <see cref="Video"/>.
    /// </summary>
    public record PostVideoCommand(VideoPostDTO VideoDto) : IRequest<ServiceResponse<int>>;
}
