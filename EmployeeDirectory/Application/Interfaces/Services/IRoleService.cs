using Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;

namespace EmployeeDirectory.Application.Interfaces;

public interface IRoleService
{
    Task<RoleResponseDTO> AddRoleAsync(RoleRequestDTO request);

    Task<List<RoleResponseDTO>> GetAllRolesAsync();

    Task<List<RoleResponseDTO>> GetRolesByDepartmentAndLocationAsync(int departmentId, int locationId);

    Task<RoleResponseDTO> GetRoleByEmployeeIdAsync(int employeeId);

    Task<int> GetRoleIdByNameAsync(string roleName);

    Task<RoleResponseDTO> GetRoleByNameAsync(string roleName);

    Task<List<RoleCardResponseDTO>> GetRoleCardsAsync();

    Task<bool> IsRoleExistsAsync(int departmentId, string roleName);
}