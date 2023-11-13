using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.User.Name).Name();
            RuleFor(x => x.User.BirthDate).BirthDate();
            RuleFor(x => x.User.Email).EmailAddress().Length(0, 100);
            RuleFor(x => x.User.Password).Password();
        }
    }
}
