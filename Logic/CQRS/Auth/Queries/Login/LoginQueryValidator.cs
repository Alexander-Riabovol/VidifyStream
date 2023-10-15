using FluentValidation;

namespace VidifyStream.Logic.CQRS.Auth.Queries.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Email).EmailAddress().Length(0, 100);
            // We check password only not to be empty because
            // password validation could change and old passwords won't work all of a sudden.
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
