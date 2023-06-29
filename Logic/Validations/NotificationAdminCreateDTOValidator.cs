using Data.Context;
using Data.Dtos.Notification;
using FluentValidation;

namespace Logic.Validations
{
    public class NotificationAdminCreateDTOValidator : AbstractValidator<NotificationAdminCreateDTO>
    {
        public NotificationAdminCreateDTOValidator(DataContext dataContext)
        {
            RuleFor(x => x.Message).NotEmpty().Length(0, 250);
            RuleFor(x => x.UserId).MustAsync(async (userId, cancellation) =>
            {
                var user = await dataContext.Users.FindAsync(userId);
                return user != null;
            }).WithMessage("A user with {PropertyValue} ID does not exist.");
        }
    }
}
