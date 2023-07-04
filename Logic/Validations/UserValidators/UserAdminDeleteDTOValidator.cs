using Data.Dtos.User;
using FluentValidation;

namespace Logic.Validations.UserValidators
{
    public class UserAdminDeleteDTOValidator : AbstractValidator<UserAdminDeleteDTO>
    {
        public UserAdminDeleteDTOValidator()
        {
            RuleFor(x => x.BanMessage).NotEmpty().Length(0, 1000);
        }
    }
}
