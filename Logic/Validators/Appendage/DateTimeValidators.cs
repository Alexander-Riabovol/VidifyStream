using FluentValidation;
using FluentValidation.Results;

namespace Logic.Validators.Appendage
{
    /// <summary>
    /// Provides custom validation rules for the type <see cref="DateTime"/>.
    /// </summary>
    internal static class DateTimeValidators
    {
        /// <summary>
        /// Validates a birth date of type <see cref="DateTime"/>?.
        /// </summary>
        public static IRuleBuilderOptionsConditions<T, DateTime?> BirthDate<T>(
            this IRuleBuilderInitial<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder.Custom((date, context) =>
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
        }

        /// <summary>
        /// Validates a birth date of type <see cref="DateTime"/>.
        /// </summary>
        public static IRuleBuilderOptionsConditions<T, DateTime> BirthDate<T>(
            this IRuleBuilderInitial<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Custom((date, context) =>
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
        }
    }
}
