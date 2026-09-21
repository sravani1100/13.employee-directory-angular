using EmployeeDirectory.Domain.Entities;
using FluentValidation;

namespace EmployeeDirectory.Application.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.EmployeeNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.FirstName)
            .ValidName();

        RuleFor(x => x.LastName)
            .ValidName();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.MobileNumber)
            .Matches(@"^[6-9]\d{9}$")
            .When(x => !string.IsNullOrWhiteSpace(x.MobileNumber))
            .WithMessage("Mobile Number must be 10 digits and start with 6, 7, 8 or 9.");

        RuleFor(x => x.JoiningDate)
            .GreaterThan(x => x.DateOfBirth)
            .WithMessage("Joining Date must be greater than Date of Birth.");
    }
}

