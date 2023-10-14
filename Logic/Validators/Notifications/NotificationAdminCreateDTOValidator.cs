using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos.Notification;
using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.Validators.Notifications
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
