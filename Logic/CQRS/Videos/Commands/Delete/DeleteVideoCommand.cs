using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Delete
{
    /// <summary>
    /// In case the Admin parameter is false, deletes a <see cref="Video"/> by its ID,
    /// if it's the authenticated <see cref="User"/>'s video.
    /// Otherwise, deletes any <see cref="Video"/>.
    /// </summary>
    public record DeleteVideoCommand(int VideoId, bool Admin) : IRequest<ServiceResponse>;
}
