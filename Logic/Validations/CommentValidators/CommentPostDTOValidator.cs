using Data.Context;
using Data.Dtos.Comment;
using FluentValidation;

namespace Logic.Validations.CommentValidators
{
    public class CommentPostDTOValidator : AbstractValidator<CommentPostDTO>
    {
        public CommentPostDTOValidator(DataContext dataContext)
        {
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.VideoId).MustAsync(async (videoId, cancellation) =>
            {
                var video = await dataContext.Videos.FindAsync(videoId);
                return video != null;
            }).WithMessage("A video with {PropertyValue} ID does not exist.");
        }
    }
}
