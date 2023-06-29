using Data.Context;
using Data.Dtos.Comment;
using FluentValidation;

namespace Logic.Validations
{
    public class ReplyPostDTOValidator : AbstractValidator<ReplyPostDTO>
    {
        public ReplyPostDTOValidator(DataContext dataContext) 
        {
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.RepliedToId).MustAsync(async (repliedToId, cancellation) =>
            {
                var repliedTo = await dataContext.Comments.FindAsync(repliedToId);
                return repliedTo != null;
            }).WithMessage("A comment with {PropertyValue} ID does not exist.");
        }
    }
}
