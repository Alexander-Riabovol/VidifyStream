using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;

namespace VidifyStream.Logic.CQRS.Video.Commands.Post
{
    public record PostVideoCommand(VideoPostDTO Video) : IRequest<ServiceResponse<int>>;
}
