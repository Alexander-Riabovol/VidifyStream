using FluentValidation;
using Microsoft.AspNetCore.Http;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Videos.Commands.Put
{
    public class PutVideoCommandValidator : AbstractValidator<PutVideoCommand>
    {
        public PutVideoCommandValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.VideoDto.VideoId).Id();
            RuleFor(x => x).Must(x => x.VideoDto.Title != null ||
                                      x.VideoDto.Description != null ||
                                      x.VideoDto.Category != null ||
                                      x.VideoDto.Thumbnail != null)
                           .WithMessage("Please provide at least 1 not null field.");

            RuleFor(x => x.VideoDto.Title).Length(5, 100);
            RuleFor(x => x.VideoDto.Description).NotEmpty().When(x => x.VideoDto.Description != null).Length(0, 2000);
            RuleFor(x => x.VideoDto.Thumbnail).Custom((file, content) =>
                                             content.RootContextData.Add("content-type", "image"))
                                     .SetValidator(fileValidator!);
        }
    }
}
