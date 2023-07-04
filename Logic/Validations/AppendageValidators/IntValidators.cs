using FluentValidation;

namespace Logic.Validations.AppendageValidators
{
    internal static class IntValidators
    {
        public static IRuleBuilderOptions<T, int> Id<T>(
            this IRuleBuilderInitial<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0)
                              .WithMessage("'{PropertyName}' must be positive. You entered {PropertyValue}.");
        }
    }
}
