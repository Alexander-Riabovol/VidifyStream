using MediatR;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.CQRS.Video.Commands.Put
{
    public record PutVideoCommand(VideoPutDTO Video) : IRequest<ServiceResponse>;
}
