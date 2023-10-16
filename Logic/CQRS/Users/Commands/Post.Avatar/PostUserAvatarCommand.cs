using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.User;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Users.Commands.Post.Avatar
{
    /// <summary>
    /// Uploads a profile picture for the current <see cref="User"/>.
    /// </summary>
    public record PostUserAvatarCommand(UserAvatarPostDTO AvatarDto) 
        : IRequest<ServiceResponse<string>>;
}
