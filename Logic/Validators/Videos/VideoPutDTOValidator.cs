using Data.Dtos.Video;
using FluentValidation;
using Logic.Validators.Appendage;
using Microsoft.AspNetCore.Http;

namespace Logic.Validators.Videos
{
    public class VideoPutDTOValidator : AbstractValidator<VideoPutDTO>
    {
        public VideoPutDTOValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.VideoId).Id();
            RuleFor(x => x).Must(x => x.Title != null ||
                                      x.Description != null ||
                                      x.Category != null ||
                                      x.Thumbnail != null)
                           .WithMessage("Please provide at least 1 not null field.");

            RuleFor(x => x.Title).Length(5, 100);
            RuleFor(x => x.Description).NotEmpty().When(x => x.Description != null).Length(0, 2000);
            RuleFor(x => x.Thumbnail).Custom((file, content) =>
                                             content.RootContextData.Add("content-type", "image"))
                                     .SetValidator(fileValidator!);
        }
    }
}
