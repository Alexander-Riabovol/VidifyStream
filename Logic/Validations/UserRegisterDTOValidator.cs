using Data.Dtos.User;
using FluentValidation;
using FluentValidation.Results;

namespace Logic.Validations
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            // Regex: Only letters and spaces
            RuleFor(x => x.Name).Matches("^[a-zA-Z ]+$")
                                .WithMessage("'{PropertyName}' must contain only 'A-z' letters or spaces.")
                                .Length(1, 60);
            RuleFor(x => x.BirthDate).Custom((date, context) =>
            {
                if (date > DateTime.Now.Date)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyName,
                        $"Invalid '{context.PropertyName}': Future date detected. Please enter a valid date."));
                }
                else if (date < DateTime.Now.Date.AddYears(-100))
                {
                    context.AddFailure(new ValidationFailure(context.PropertyName,
                        $"Invalid '{context.PropertyName}': Date exceeds allowable range. Please provide a valid '{context.PropertyName}' within the past century."));
                }
            });
            RuleFor(x => x.Email).EmailAddress().Length(0, 100);
            // Regex: At least 1 number, 1 uppercase letter, 1 lowercase letter
            RuleFor(x => x.Password).Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$")
                                    .WithMessage("You entered a weak password. '{PropertyName}' must contain at least 1 number, 1 uppercase letter and 1 lowercase letter.")
                                    .Length(8, 60);
        }
    }
}
