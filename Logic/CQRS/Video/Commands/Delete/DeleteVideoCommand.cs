using MediatR;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.CQRS.Video.Commands.Delete
{
    public record DeleteVideoCommand(int VideoId, bool Admin) : IRequest<ServiceResponse>;
}
