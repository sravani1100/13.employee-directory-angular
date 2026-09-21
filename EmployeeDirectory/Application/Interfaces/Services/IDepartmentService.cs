using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentResponseDTO> AddDepartmentAsync(DepartmentRequestDTO request);

    Task<List<string>> GetAllAsync();

    Task<List<DepartmentResponseDTO>> GetAllDepartmentsAsync();

    Task<List<string>> GetDepartmentsAsync();

    Task<DepartmentResponseDTO> GetDepartmentByLocationIdAsync(int locationId);

    Task<int> GetDepartmentIdByNameAsync(string departmentName);

    Task<DepartmentResponseDTO> GetDepartmentByIdAsync(int departmentId);
}

