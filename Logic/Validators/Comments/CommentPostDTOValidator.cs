using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos.Comment;
using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.Validators.Comments
{
    public class CommentPostDTOValidator : AbstractValidator<CommentPostDTO>
    {
        public CommentPostDTOValidator(DataContext dataContext)
        {
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.VideoId).Id().MustAsync(async (videoId, cancellation) =>
            {
                var video = await dataContext.Videos.FindAsync(videoId);
                return video != null;
            }).WithMessage("A video with {PropertyValue} ID does not exist.");
        }
    }
}
