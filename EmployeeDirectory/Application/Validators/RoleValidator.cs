using EmployeeDirectory.Domain.Entities;
using FluentValidation;

namespace EmployeeDirectory.Application.Validators;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(x => x.RoleName)
            .ValidName();
    }
}

