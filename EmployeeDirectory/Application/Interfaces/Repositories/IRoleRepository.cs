using Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role> AddAsync(Role role);

    Task<List<Role>> GetAllAsync();

    Task<Role?> GetRoleByEmployeeIdAsync(int employeeId);

    Task<int> GetRoleIdByNameAsync(string roleName);

    Task<Role?> GetRoleByNameAsync(string roleName);

    Task<List<Role>> GetRoleCardsAsync();

    Task<bool> IsRoleExistsAsync(int departmentId, string roleName);
}
