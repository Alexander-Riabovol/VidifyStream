using FluentValidation;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Dtos.Video;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Post
{
    public class PostVideoCommandValidator : AbstractValidator<PostVideoCommand>
    {
        public PostVideoCommandValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.VideoDto.Title).Length(5, 100);
            RuleFor(x => x.VideoDto.Description).NotEmpty().Length(0, 2000);
            RuleFor(x => x.VideoDto.Thumbnail).Custom((file, content) =>
                                             content.RootContextData.Add("content-type", "image"))
                                     .SetValidator(fileValidator!);
            //.When(x => x.Thumbnail != null);
            RuleFor(x => x.VideoDto.VideoFile).Custom((file, content) =>
            {
                //if(!content.RootContextData.TryAdd("content-type", "video"))
                //{
                content.RootContextData["content-type"] = "video";
                //}
            }).SetValidator(fileValidator);
        }
    }
}
