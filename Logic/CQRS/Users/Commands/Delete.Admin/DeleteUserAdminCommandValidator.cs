using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Users.Commands.Delete.Admin
{
    public class DeleteUserAdminCommandValidator : AbstractValidator<DeleteUserAdminCommand>
    {
        public DeleteUserAdminCommandValidator()
        {
            RuleFor(x => x.UserDto.UserId).Id();
            RuleFor(x => x.UserDto.BanMessage).NotEmpty().Length(0, 1000);
        }
    }
}
