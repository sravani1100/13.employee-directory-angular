using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee);

    Task<Employee?> GetEmployeeByIdAsync(int? employeeId);

    Task<List<Employee>> GetAllAsync();

    Task<List<Employee>> GetEmployeeDetailsAsync();

    Task<Employee?> GetEmployeeByNumberAsync(string employeeNumber);

    Task<List<string>> GetAllManagersAsync();

    Task<List<Employee>> GetEmployeesByManagerIdAsync(int managerId);

    Task<Employee?> GetEmployeeByEmailAsync(string email);

    Task<Employee> UpdateAsync(Employee updatedEmployee);
    
    Task<bool> DeleteAsync(string employeeNumber);

    Task<bool> IsEmailExistsAsync(string email);
}
