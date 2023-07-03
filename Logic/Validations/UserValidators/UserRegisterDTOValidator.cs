using Data.Dtos.User;
using FluentValidation;
using Logic.Validations.AppendageValidators;

namespace Logic.Validations.UserValidators
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
