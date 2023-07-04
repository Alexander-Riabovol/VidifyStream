using Data.Dtos.Comment;
using FluentValidation;
using Logic.Validations.AppendageValidators;

namespace Logic.Validations.CommentValidators
{
    public class CommentPutDTOValidator : AbstractValidator<CommentPutDTO>
    {
        public CommentPutDTOValidator()
        {
            RuleFor(x => x.CommentId).Id();
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
        }
    }
}
