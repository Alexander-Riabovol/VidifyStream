using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Video;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Put
{
    /// <summary>
    /// Updates a <see cref="Video"/>'s information.
    /// </summary>
    public record PutVideoCommand(VideoPutDTO VideoDto) : IRequest<ServiceResponse>;
}
