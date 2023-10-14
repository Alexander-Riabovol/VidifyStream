using VidifyStream.Data.Dtos.User;
using FluentValidation;

namespace VidifyStream.Logic.Validators.Users
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().Length(0, 100);
            // We check password only not to be empty because
            // password validation could change and old passwords won't work all of a sudden.
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
