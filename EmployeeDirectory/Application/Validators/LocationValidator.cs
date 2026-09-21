using EmployeeDirectory.Domain.Entities;
using FluentValidation;

namespace EmployeeDirectory.Application.Validators;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.LocationName)
            .ValidName();
    }
}

