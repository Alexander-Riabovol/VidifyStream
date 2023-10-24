using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Put
{
    public class PutCommentCommandValidator : AbstractValidator<PutCommentCommand>
    {
        public PutCommentCommandValidator()
        {
            RuleFor(x => x.CommentDto.CommentId).Id();
            RuleFor(x => x.CommentDto.Text).NotEmpty().Length(0, 250);
        }
    }
}
