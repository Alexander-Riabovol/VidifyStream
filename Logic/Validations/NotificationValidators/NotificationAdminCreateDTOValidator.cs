using Data.Context;
using Data.Dtos.Notification;
using FluentValidation;
using Logic.Validations.AppendageValidators;

namespace Logic.Validations.Notification
{
    public class NotificationAdminCreateDTOValidator : AbstractValidator<NotificationAdminCreateDTO>
    {
        public NotificationAdminCreateDTOValidator(DataContext dataContext)
        {
            RuleFor(x => x.Message).NotEmpty().Length(0, 250);
            RuleFor(x => x.UserId).Id().MustAsync(async (userId, cancellation) =>
            {
                var user = await dataContext.Users.FindAsync(userId);
                return user != null;
            }).WithMessage("A user with {PropertyValue} ID does not exist.");
        }
    }
}
