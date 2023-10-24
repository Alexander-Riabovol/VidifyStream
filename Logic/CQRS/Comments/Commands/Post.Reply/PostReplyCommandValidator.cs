using FluentValidation;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Post.Reply
{
    public class PostReplyCommandValidator : AbstractValidator<PostReplyCommand>
    {
        public PostReplyCommandValidator(DataContext dataContext)
        {
            RuleFor(x => x.ReplyDto.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.ReplyDto.RepliedToId).Id().MustAsync(async (repliedToId, cancellation) =>
            {
                var repliedTo = await dataContext.Comments.FindAsync(repliedToId);
                return repliedTo != null;
            }).WithMessage("A comment with {PropertyValue} ID does not exist.");
        }
    }
}
