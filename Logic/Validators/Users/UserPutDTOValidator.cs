using Data.Dtos.User;
using FluentValidation;
using Logic.Validators.Appendage;

namespace Logic.Validators.Users
{
    public class UserPutDTOValidator : AbstractValidator<UserPutDTO>
    {
        public UserPutDTOValidator()
        {
            RuleFor(x => x).Must(x => x.Name != null || x.BirthDate != null || x.Bio != null)
                           .WithMessage("Please provide at least 1 not null field.");
            RuleFor(x => x.Name!).Name();
            RuleFor(x => x.BirthDate).BirthDate();
            RuleFor(x => x.Bio).Length(1, 250);
        }
    }
}
