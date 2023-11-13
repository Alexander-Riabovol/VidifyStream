using FluentValidation;
using VidifyStream.Data.Context;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post
{
    public class PostCommentCommandValidator : AbstractValidator<PostCommentCommand>
    {
        public PostCommentCommandValidator(DataContext dataContext)
        {
            RuleFor(x => x.CommentDto.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.CommentDto.VideoId).Id().MustAsync(async (videoId, cancellation) =>
            {
                var video = await dataContext.Videos.FindAsync(videoId);
                return video != null;
            }).WithMessage("A video with {PropertyValue} ID does not exist.");
        }
    }
}