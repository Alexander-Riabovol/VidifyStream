using VidifyStream.Data.Dtos.User;
using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.Validators.Users
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
