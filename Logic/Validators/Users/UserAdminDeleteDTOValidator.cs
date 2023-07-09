using Data.Dtos.User;
using FluentValidation;
using Logic.Validators.Appendage;

namespace Logic.Validators.Users
{
    public class UserAdminDeleteDTOValidator : AbstractValidator<UserAdminDeleteDTO>
    {
        public UserAdminDeleteDTOValidator()
        {
            RuleFor(x => x.UserId).Id();
            RuleFor(x => x.BanMessage).NotEmpty().Length(0, 1000);
        }
    }
}
