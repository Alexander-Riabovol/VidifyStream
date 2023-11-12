using FluentValidation;
using VidifyStream.Data.Context;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.CQRS.Notifications.Commands.Push.Admin
{
    public class PushNotificationAdminCommandValidator : AbstractValidator<PushNotificationAdminCommand>
    {
        public PushNotificationAdminCommandValidator(DataContext dataContext)
        {
            RuleFor(x => x.NotificationDto.Message).NotEmpty().Length(0, 250);
            RuleFor(x => x.NotificationDto.UserId).Id().MustAsync(async (userId, cancellation) =>
            {
                var user = await dataContext.Users.FindAsync(userId);
                return user != null;
            }).WithMessage("A user with {PropertyValue} ID does not exist.");
        }
    }
}
