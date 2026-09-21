using EmployeeDirectory.Domain.Entities;
using FluentValidation;

namespace EmployeeDirectory.Application.Validators;

public class DepartmentValidator : AbstractValidator<Department>
{
    public DepartmentValidator()
    {
        RuleFor(x => x.DepartmentName)
            .ValidName();
    }

}

