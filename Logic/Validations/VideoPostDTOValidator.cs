using Data.Dtos.Video;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Logic.Validations
{
    public class VideoPostDTOValidator : AbstractValidator<VideoPostDTO>
    {
        public VideoPostDTOValidator(IValidator<IFormFile> fileValidator) 
        {
            RuleFor(x => x.Title).Length(5, 100);
            RuleFor(x => x.Description).Length(1, 2000);
            RuleFor(x => x.Thumbnail).Custom((file, content) =>
                                             content.RootContextData.Add("content-type", "image"))
                                     .SetValidator(fileValidator!);
                                     //.When(x => x.Thumbnail != null);
            RuleFor(x => x.VideoFile).Custom((file, content) =>
            {
                //if(!content.RootContextData.TryAdd("content-type", "video"))
                //{
                    content.RootContextData["content-type"] = "video";
                //}
            }).SetValidator(fileValidator);
        }
    }
}
