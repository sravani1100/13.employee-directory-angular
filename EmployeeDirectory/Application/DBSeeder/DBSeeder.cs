using Application.DTO.RequestDTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.DBSeeder;

public class DBSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IEmployeeService _employeeService;
    private readonly AdminSettingsDTO _adminSettingsDTO;

    public DBSeeder(IUserRepository userRepository,
        IEmployeeService employeeService,
        IOptions<AdminSettingsDTO> adminSettingsDTO)
    {
        _userRepository = userRepository;
        _employeeService = employeeService;
        _adminSettingsDTO = adminSettingsDTO.Value;
    }
    public async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {

        User? existingUser =
            await _userRepository.GetByUserNameAsync(_adminSettingsDTO.Email);

        if (existingUser != null)
            return;

        EmployeeRequestDTO employeeRequest = new EmployeeRequestDTO
        {
            FirstName = "Ram",
            LastName = "Babu",
            Email = _adminSettingsDTO.Email,
            DateOfBirth = DateTime.MinValue,
            MobileNumber = "9999999999",
            JoiningDate = DateTime.MinValue,
            ManagerId = null,

            // Give values required by your AddEmployeeAsync
            RoleName = "Admin",
            ProjectName = "Employee Directory",
            LocationName = "Hyderabad",

            Password = _adminSettingsDTO.Password
        };

        await _employeeService.AddEmployeeAsync(employeeRequest);
    }
}

