using FluentValidation;

namespace EmployeeDirectory.Application.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
           .NotEmpty()
               .WithMessage("{PropertyName} cannot be empty.")
           .MaximumLength(20)
               .WithMessage("{PropertyName} should contain a maximum of 20 characters.")
           .Matches(@"^[A-Za-z ]+$")
               .WithMessage("{PropertyName} should contain only alphabets.")
           .Must(name => name.Count(c => c == ' ') <= 1)
               .WithMessage("{PropertyName} can contain only one space.");
    }
}

