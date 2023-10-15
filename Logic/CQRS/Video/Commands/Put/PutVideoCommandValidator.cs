using FluentValidation;
using Microsoft.AspNetCore.Http;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Video.Commands.Put
{
    public class PutVideoCommandValidator : AbstractValidator<PutVideoCommand>
    {
        public PutVideoCommandValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.Video.VideoId).Id();
            RuleFor(x => x).Must(x => x.Video.Title != null ||
                                      x.Video.Description != null ||
                                      x.Video.Category != null ||
                                      x.Video.Thumbnail != null)
                           .WithMessage("Please provide at least 1 not null field.");

            RuleFor(x => x.Video.Title).Length(5, 100);
            RuleFor(x => x.Video.Description).NotEmpty().When(x => x.Video.Description != null).Length(0, 2000);
            RuleFor(x => x.Video.Thumbnail).Custom((file, content) =>
                                             content.RootContextData.Add("content-type", "image"))
                                     .SetValidator(fileValidator!);
        }
    }
}
