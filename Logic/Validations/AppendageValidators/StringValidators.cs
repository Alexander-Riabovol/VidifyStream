using FluentValidation;

namespace Logic.Validations.AppendageValidators
{
    internal static class StringValidators
    {
        public static IRuleBuilderOptions<T, string> Name<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            // Regex: Only letters and spaces
            return ruleBuilder.Matches("^[a-zA-Z ]+$")
                              .WithMessage("'{PropertyName}' must contain only 'A-z' letters or spaces.")
                              .Length(1, 60);
        }

        public static IRuleBuilderOptions<T, string> Password<T>(
            this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            // Regex: At least 1 number, 1 uppercase letter, 1 lowercase letter
            return ruleBuilder.Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$")
                              .WithMessage("You entered a weak password. '{PropertyName}' must contain at least 1 number, 1 uppercase letter and 1 lowercase letter.")
                              .Length(8, 60);
        }
    }
}
