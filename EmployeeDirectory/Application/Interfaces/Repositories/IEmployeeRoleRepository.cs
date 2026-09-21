using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IEmployeeRoleRepository
{
    Task AddEmployeeRoleAsync(EmployeeRole employeeRole);

    Task DeleteByEmployeeIdAsync(int employeeId);

    Task<bool> IsExistsAsync(int employeeId, int roleId);
}

