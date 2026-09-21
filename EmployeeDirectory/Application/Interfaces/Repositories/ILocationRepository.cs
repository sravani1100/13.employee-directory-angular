using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<List<Location>> GetAllAsync();

    Task<Location?> GetLocationByIdAsync(int locationId);

    Task<Location?> GetLocationByDepartmentIdAsync(int departmentId);

    Task<int> GetLocationIdByNameAsync(string locationName);

    Task<Location> AddAsync(Location location);

    Task<Location?> UpdateAsync(Location location);

    Task<bool> DeleteAsync(int locationId);
}


