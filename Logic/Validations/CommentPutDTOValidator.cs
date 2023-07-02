using Data.Dtos.Comment;
using FluentValidation;

namespace Logic.Validations
{
    public class CommentPutDTOValidator : AbstractValidator<CommentPutDTO>
    {
        public CommentPutDTOValidator() 
        {
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
        }
    }
}
