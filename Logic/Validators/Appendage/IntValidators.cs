using FluentValidation;

namespace Logic.Validators.Appendage
{
    /// <summary>
    /// Provides custom validation rules for the type <see cref="int"/>.
    /// </summary>
    internal static class IntValidators
    {
        /// <summary>
        /// Adds a validation error message if the property is not valid as ID.
        /// </summary>
        public static IRuleBuilderOptions<T, int> Id<T>(
            this IRuleBuilderInitial<T, int> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0)
                              .WithMessage("'{PropertyName}' must be positive. You entered {PropertyValue}.");
        }
    }
}
