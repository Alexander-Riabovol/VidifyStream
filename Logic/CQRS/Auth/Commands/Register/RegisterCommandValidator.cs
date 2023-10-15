using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Name).Name();
            RuleFor(x => x.BirthDate).BirthDate();
            RuleFor(x => x.Email).EmailAddress().Length(0, 100);
            RuleFor(x => x.Password).Password();
        }
    }
}
