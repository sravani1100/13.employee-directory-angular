using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface ILocationDepartmentRepository
{
    Task AddLocationDepartmentAsync(LocationDepartment locationDepartment);

    Task<List<LocationDepartment>> GetAllAsync();

    Task<bool> IsExistsAsync(int locationId, int departmentId);
}

