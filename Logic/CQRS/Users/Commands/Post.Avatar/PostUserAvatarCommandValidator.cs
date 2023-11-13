using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace VidifyStream.Logic.CQRS.Users.Commands.Post.Avatar
{
    public class PostUserAvatarCommandValidator : AbstractValidator<PostUserAvatarCommand>
    {
        public PostUserAvatarCommandValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.AvatarDto.File).Custom((file, content) =>
                                           content.RootContextData.Add("content-type", "image"))
                                          .SetValidator(fileValidator);
        }
    }
}
