using Data.Dtos.User;
using FluentValidation;
using Logic.Validators.Appendage;

namespace Logic.Validators.Users
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.Name).Name();
            RuleFor(x => x.BirthDate).BirthDate();
            RuleFor(x => x.Email).EmailAddress().Length(0, 100);
            RuleFor(x => x.Password).Password();
        }
    }
}
