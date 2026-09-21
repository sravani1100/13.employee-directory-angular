using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IEmployeeProjectRepository
{
    Task AddEmployeeProjectAsync(EmployeeProject employeeProject);

    Task DeleteByEmployeeIdAsync(int employeeId);

    Task<bool> IsExistsAsync(int employeeId, int projectId);
}

