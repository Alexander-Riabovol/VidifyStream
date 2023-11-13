using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Users.Commands.Put
{
    public class PutUserCommandValidator : AbstractValidator<PutUserCommand>
    {
        public PutUserCommandValidator()
        {
            RuleFor(x => x).Must(x => x.UserDto.Name != null ||
                                 x.UserDto.BirthDate != null ||
                                 x.UserDto.Bio != null)
                           .WithMessage("Please provide at least 1 not null field.");
            RuleFor(x => x.UserDto.Name!).Name();
            RuleFor(x => x.UserDto.BirthDate).BirthDate();
            RuleFor(x => x.UserDto.Bio).Length(1, 250);
        }
    }
}
