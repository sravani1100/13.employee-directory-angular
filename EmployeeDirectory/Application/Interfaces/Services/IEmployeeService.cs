using Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;

namespace EmployeeDirectory.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeResponseDTO> AddEmployeeAsync(EmployeeRequestDTO request);

    Task<EmployeeDetailsResponseDTO> GetEmployeeByIdAsync(int employeeId);

    Task<List<EmployeeResponseDTO>> GetAllEmployeesAsync();

    Task<EmployeeResponseDTO> GetEmployeeByNumberAsync(string employeeNumber);

    Task<List<string>> GetAllManagersAsync();

    Task<EmployeeDetailsResponseDTO> UpdateEmployeeByNumberAsync(string employeeNumber, EmployeeRequestDTO request);

    Task<bool> DeleteEmployeeByNumberAsync(string employeeNumber);

    Task<bool> IsEmailExistsAsync(string email);

}

