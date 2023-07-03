using FluentValidation;
using FluentValidation.Results;

namespace Logic.Validations.AppendageValidators
{
    internal static class DateTimeValidators
    {
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
