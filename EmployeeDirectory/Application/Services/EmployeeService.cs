using Application.DTO.RequestDTOs;
using Application.DTO.ResponseDTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Repositories.Interfaces;
using EmployeeDirectory.Application.Repositories.Repositories;
using EmployeeDirectory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectory.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository;
    private readonly IEmployeeProjectRepository _employeeProjectRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        ILocationRepository locationRepository,
        IRoleRepository roleRepository,
        IProjectRepository projectRepository,
        IEmployeeRoleRepository employeeRoleRepository,
        IEmployeeProjectRepository employeeProjectRepository,
        IUserRepository userRepository,
        ILogger<EmployeeService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _locationRepository = locationRepository;
        _roleRepository = roleRepository;
        _projectRepository = projectRepository;
        _employeeRoleRepository = employeeRoleRepository;
        _employeeProjectRepository = employeeProjectRepository;
        _userRepository = userRepository;
        _logger = logger;
    }


    public async Task<EmployeeResponseDTO> AddEmployeeAsync(EmployeeRequestDTO request)
    {
        _logger.LogInformation(
            "Adding employee with email {Email}.",
            request?.Email);

        string initialPassword = $"{ request?.FirstName}.{ request?.LastName.FirstOrDefault()}@tezo";

        if (request == null)
        {
            _logger.LogWarning(
                "Employee creation failed. Request is null.");

            throw new InvalidFieldException(
                "Cannot add employee. Give required details to add employee.");
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            int locationId =
                await _locationRepository.GetLocationIdByNameAsync(request.LocationName);
            if (locationId == 0)
            {
                _logger.LogWarning(
                    "Location {LocationName} not found.",
                    request.LocationName);

                throw new ResourceNotFoundException(
                    "Location not found.");
            }

            int roleId = await _roleRepository.GetRoleIdByNameAsync(request.RoleName);
            if (roleId == 0)
            {
                _logger.LogWarning(
                    "Role {RoleName} not found.",
                    request.RoleName);

                throw new ResourceNotFoundException(
                    "Role not found.");
            }

            int projectId = await _projectRepository.GetProjectIdByNameAsync(request.ProjectName);
            if (projectId == 0)
            {
                _logger.LogWarning(
                    "Project {ProjectName} not found.",
                    request.ProjectName);

                throw new ResourceNotFoundException(
                    "Project not found.");
            }

            Employee employee = _mapper.Map<Employee>(request);

            employee.LocationId = locationId;
            employee.EmployeeNumber = await GenerateEmployeeNumberAsync();

            employee = await _employeeRepository.AddAsync(employee);

            await _unitOfWork.SaveChangesAsync();

            UserRequestDTO userDto = new UserRequestDTO
            {
                EmployeeId = employee.EmployeeId,
                EmployeeNumber = employee.EmployeeNumber,
                UserName = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(initialPassword),
                RoleId = roleId,
                RoleName = request.RoleName
            };

            User user = _mapper.Map<User>(userDto);
            await _userRepository.AddAsync(user);

            if (!await _employeeRoleRepository
                .IsExistsAsync(employee.EmployeeId, roleId))
            {
                await _employeeRoleRepository.AddEmployeeRoleAsync(
                    new EmployeeRole
                    {
                        EmployeeId = employee.EmployeeId,
                        RoleId = roleId
                    });
            }

            if (!await _employeeProjectRepository
                .IsExistsAsync(employee.EmployeeId, projectId))
            {
                await _employeeProjectRepository.AddEmployeeProjectAsync(
                    new EmployeeProject
                    {
                        EmployeeId = employee.EmployeeId,
                        ProjectId = projectId
                    });
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation(
                "Employee created successfully. EmployeeId {EmployeeId}, EmployeeNumber {EmployeeNumber}.",
                employee.EmployeeId,
                employee.EmployeeNumber);

            Employee employeeDetails =
                (await _employeeRepository.GetEmployeeDetailsAsync())
                .First(e => e.EmployeeId == employee.EmployeeId);

            return _mapper.Map<EmployeeResponseDTO>(employeeDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Employee creation failed. Transaction rollback started.");

            await _unitOfWork.RollBackTransactionAsync();

            throw;
        }
    }

    public async Task<EmployeeDetailsResponseDTO> GetEmployeeByIdAsync(int employeeId)
    {
        _logger.LogInformation(
            "Fetching employee by Id {EmployeeId}.",
            employeeId);

        if (employeeId <= 0)
        {
            _logger.LogWarning(
                "Invalid employee Id {EmployeeId}.",
                employeeId);

            throw new InvalidFieldException(
                "Please provide valid employee id.");
        }

        Employee? employee =
            await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if (employee == null)
        {
            _logger.LogWarning(
                "Employee {EmployeeId} not found.",
                employeeId);

            throw new ResourceNotFoundException(
                "Employee Not Found.");
        }
        return _mapper.Map<EmployeeDetailsResponseDTO>(employee);
    }

    public async Task<List<EmployeeResponseDTO>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Fetching all employees.");

        List<Employee> employees =
            await _employeeRepository.GetEmployeeDetailsAsync();
        if (employees.Count == 0)
        {
            _logger.LogWarning(
                "No employees found.");

            throw new ResourceNotFoundException(
                "No Employee Found.");
        }

        _logger.LogInformation(
            "Retrieved {EmployeeCount} employees.",
            employees.Count);

        return _mapper.Map<List<EmployeeResponseDTO>>(employees);
    }

    public async Task<EmployeeResponseDTO> GetEmployeeByNumberAsync(string employeeNumber)
    {
        _logger.LogInformation(
            "Fetching employee {EmployeeNumber}.",
            employeeNumber);

        if (string.IsNullOrWhiteSpace(employeeNumber))
        {
            _logger.LogWarning(
                "Invalid employee number.");

            throw new InvalidFieldException(
                "Invalid Employee Number.");
        }

        Employee? employee =
            await _employeeRepository.GetEmployeeByNumberAsync(employeeNumber);
        if (employee == null)
        {
            _logger.LogWarning(
                "Employee {EmployeeNumber} not found.",
                employeeNumber);

            throw new ResourceNotFoundException(
                "Employee Not Found.");
        }
        return _mapper.Map<EmployeeResponseDTO>(employee);
    }

    public async Task<List<string>> GetAllManagersAsync()
    {
        _logger.LogInformation(
            "Fetching all managers.");

        return await _employeeRepository.GetAllManagersAsync();
    }

    public async Task<EmployeeDetailsResponseDTO> UpdateEmployeeByNumberAsync(
    string employeeNumber,
    EmployeeRequestDTO request)
    {
        _logger.LogInformation(
            "Updating employee {EmployeeNumber}.",
            employeeNumber);

        if (request == null)
        {
            throw new InvalidFieldException("Invalid Employee Details.");
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            Employee? employee =
                await _employeeRepository.GetEmployeeByNumberAsync(employeeNumber);

            if (employee == null)
                throw new ResourceNotFoundException("Employee Not Found.");

            Employee? existingEmployee =
                await _employeeRepository.GetEmployeeByEmailAsync(request.Email);

            if (existingEmployee != null &&
                existingEmployee.EmployeeId != employee.EmployeeId)
            {
                throw new InvalidFieldException("Email already exists.");
            }

            if (request.ManagerId.HasValue)
            {
                Employee? manager =
                    await _employeeRepository.GetEmployeeByIdAsync(request.ManagerId.Value);

                if (manager == null)
                {
                    throw new InvalidFieldException("Invalid Manager.");
                }
            }

            int locationId =
                await _locationRepository.GetLocationIdByNameAsync(request.LocationName);

            if (locationId == 0)
            {
                throw new ResourceNotFoundException("Location not found.");
            }

            int roleId =
                await _roleRepository.GetRoleIdByNameAsync(request.RoleName);

            if (roleId == 0)
            {
                throw new ResourceNotFoundException("Role not found.");
            }

            int projectId =
                await _projectRepository.GetProjectIdByNameAsync(request.ProjectName);

            if (projectId == 0)
            {
                throw new ResourceNotFoundException("Project not found.");
            }

          
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.DateOfBirth = request.DateOfBirth;
            employee.Email = request.Email;
            employee.MobileNumber = request.MobileNumber;
            employee.JoiningDate = request.JoiningDate;
            employee.Status = request.Status;
            employee.ManagerId = request.ManagerId;
            employee.LocationId = locationId;

            await _employeeRepository.UpdateAsync(employee);

         
            User? user =
                await _userRepository.GetByEmployeeIdAsync(employee.EmployeeId);

            if (user != null)
            {
                user.UserName = request.Email;

                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    user.PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword(request.Password);
                }
            }

            await _employeeRoleRepository.DeleteByEmployeeIdAsync(employee.EmployeeId);
            await _employeeRoleRepository.AddEmployeeRoleAsync(
                new EmployeeRole
                {
                    EmployeeId = employee.EmployeeId,
                    RoleId = roleId
                });

            await _employeeProjectRepository.DeleteByEmployeeIdAsync(employee.EmployeeId);
            await _employeeProjectRepository.AddEmployeeProjectAsync(
                new EmployeeProject
                {
                    EmployeeId = employee.EmployeeId,
                    ProjectId = projectId
                });

            await _unitOfWork.SaveChangesAsync();


            Employee? updatedEmployee =
                await _employeeRepository.GetEmployeeByIdAsync(employee.EmployeeId);


            EmployeeDetailsResponseDTO response =
                _mapper.Map<EmployeeDetailsResponseDTO>(updatedEmployee);


            await _unitOfWork.CommitTransactionAsync();


            _logger.LogInformation(
                "Employee {EmployeeNumber} updated successfully.",
                employeeNumber);


            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating employee {EmployeeNumber}.",
                employeeNumber);

            await _unitOfWork.RollBackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteEmployeeByNumberAsync(string employeeNumber)
    {
        _logger.LogInformation(
            "Deleting employee {EmployeeNumber}.",
            employeeNumber);

        if (string.IsNullOrWhiteSpace(employeeNumber))
        {
            throw new InvalidFieldException(
                "Invalid Employee Number.");
        }


        Employee? employee =
            await _employeeRepository.GetEmployeeByNumberAsync(employeeNumber);


        if (employee == null)
        {
            _logger.LogWarning(
                "Employee {EmployeeNumber} not found.",
                employeeNumber);

            return false;
        }


        // Remove manager reference from reporting employees
        List<Employee> reportingEmployees =
            await _employeeRepository
                .GetEmployeesByManagerIdAsync(employee.EmployeeId);


        foreach (var reportingEmployee in reportingEmployees)
        {
            reportingEmployee.ManagerId = null;
        }


        // Delete user record first
        User? user =
            await _userRepository.GetByEmployeeIdAsync(employee.EmployeeId);


        if (user != null)
        {
            await _userRepository.DeleteAsync(user);
        }


        // Delete employee related roles/projects if required
        await _employeeRoleRepository
            .DeleteByEmployeeIdAsync(employee.EmployeeId);


        await _employeeProjectRepository
            .DeleteByEmployeeIdAsync(employee.EmployeeId);


        // Delete employee
        bool isDeleted =
            await _employeeRepository.DeleteAsync(employeeNumber);


        if (!isDeleted)
        {
            return false;
        }


        await _unitOfWork.SaveChangesAsync();


        _logger.LogInformation(
            "Employee {EmployeeNumber} deleted successfully.",
            employeeNumber);


        return true;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        _logger.LogInformation(
            "Checking email existence for {Email}.",
            email);

        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning(
                "Email validation failed.");

            throw new InvalidFieldException(
                "Missing email id.Please provide email id.");
        }
        return await _employeeRepository.IsEmailExistsAsync(email);
    }

    private async Task<string> GenerateEmployeeNumberAsync()
    {
        List<Employee> employees = await _employeeRepository.GetAllAsync();
        while (true)
        {
            Guid guid = Guid.NewGuid();

            int hash = Math.Abs(guid.GetHashCode());
            int number = hash % 10000;

            string newId = $"TZ{number:D4}";
            if (!employees.Any(e => e.EmployeeNumber == newId))
            {
                _logger.LogDebug("Generated employee number {EmployeeNumber}.", newId);
                return newId;
            }
        }
    }
}
