using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<Department> AddAsync(Department department);

    Task<List<Department>> GetAllAsync();

    Task<Department?> GetDepartmentByLocationIdAsync(int locationId);

    Task<int> GetDepartmentIdByNameAsync(string departmentName);

    Task<Department?> GetDepartmentByIdAsync(int departmentId);
}

