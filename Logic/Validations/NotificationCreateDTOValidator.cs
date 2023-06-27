using Data.Dtos.Notification;
using FluentValidation;

namespace Logic.Validations
{
    public class NotificationCreateDTOValidator : AbstractValidator<NotificationCreateDTO>
    {
        public NotificationCreateDTOValidator()
        {
            RuleFor(x => x.Message).NotNull().NotEmpty().Length(0, 250);
        }
    }
}
