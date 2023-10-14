using VidifyStream.Data.Dtos.User;
using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.Validators.Users
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
