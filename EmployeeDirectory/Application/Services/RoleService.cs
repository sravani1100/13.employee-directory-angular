using Application.DTO.ResponseDTOs;
using AutoMapper;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Repositories.Interfaces;
using EmployeeDirectory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectory.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILocationDepartmentRepository _locationDepartmentRepository;
    private readonly ILocationService _locationService;
    private readonly IDepartmentService _departmentService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleService> _logger;

    public RoleService(
        IRoleRepository roleRepository,
        ILocationDepartmentRepository locationDepartmentRepository,
        ILocationService locationService,
        IDepartmentService departmentService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RoleService> logger)
    {
        _roleRepository = roleRepository;
        _locationDepartmentRepository = locationDepartmentRepository;
        _departmentService = departmentService;
        _locationService = locationService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<RoleResponseDTO> AddRoleAsync(RoleRequestDTO request)
    {
        _logger.LogInformation(
            "Adding role {RoleName}.",
            request?.RoleName);


        if (request == null)
        {
            _logger.LogWarning(
                "Role creation failed. Request is null.");

            throw new InvalidFieldException(
                "Cannot add role. Give required details to add role.");
        }


        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            int locationId =
                await _locationService.GetLocationIdByNameAsync(request.LocationName);


            if (locationId == 0)
            {
                _logger.LogInformation(
                    "Location {LocationName} not found. Creating new location.",
                    request.LocationName);


                var location = await _locationService.AddLocationAsync(
                    new LocationRequestDTO
                    {
                        LocationName = request.LocationName
                    });


                locationId = location.LocationId;
            }


            int departmentId =
                await _departmentService.GetDepartmentIdByNameAsync(request.DepartmentName);


            if (departmentId == 0)
            {
                _logger.LogInformation(
                    "Department {DepartmentName} not found. Creating new department.",
                    request.DepartmentName);


                var department = await _departmentService.AddDepartmentAsync(
                    new DepartmentRequestDTO
                    {
                        DepartmentName = request.DepartmentName
                    });


                departmentId = department.DepartmentId;
            }

            Role role = _mapper.Map<Role>(request);
            role.DepartmentId = departmentId;

            await _roleRepository.AddAsync(role);
            if (!await _locationDepartmentRepository
                .IsExistsAsync(locationId, departmentId))
            {
                _logger.LogInformation(
                    "Creating Location-Department mapping. LocationId {LocationId}, DepartmentId {DepartmentId}.",
                    locationId,
                    departmentId);

                await _locationDepartmentRepository.AddLocationDepartmentAsync(
                    new LocationDepartment
                    {
                        LocationId = locationId,
                        DepartmentId = departmentId
                    });
            }


            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();


            _logger.LogInformation(
                "Role {RoleName} created successfully with Id {RoleId}.",
                role.RoleName,
                role.RoleId);


            return _mapper.Map<RoleResponseDTO>(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Role creation failed for {RoleName}. Rolling back transaction.",
                request.RoleName);


            await _unitOfWork.RollBackTransactionAsync();

            throw;
        }
    }


    public async Task<List<RoleResponseDTO>> GetAllRolesAsync()
    {
        _logger.LogInformation(
            "Fetching all roles.");


        List<Role> roles =
            await _roleRepository.GetAllAsync();


        if (roles.Count == 0)
        {
            _logger.LogWarning(
                "No roles found.");

            throw new ResourceNotFoundException(
                "No Role Found.");
        }


        _logger.LogInformation(
            "Retrieved {RoleCount} roles.",
            roles.Count);


        return _mapper.Map<List<RoleResponseDTO>>(roles);
    }


    public async Task<List<RoleResponseDTO>> GetRolesByDepartmentAndLocationAsync(
    int departmentId,
    int locationId)
    {
        _logger.LogInformation(
            "Fetching roles for DepartmentId {DepartmentId} and LocationId {LocationId}.",
            departmentId,
            locationId);

        List<LocationDepartment> locationDepartments =
            await _locationDepartmentRepository.GetAllAsync();

        bool isValid = locationDepartments.Any(
            ld => ld.DepartmentId == departmentId &&
                  ld.LocationId == locationId);

        if (!isValid)
        {
            _logger.LogWarning(
                "Invalid DepartmentId {DepartmentId} and LocationId {LocationId}.",
                departmentId,
                locationId);

            return new List<RoleResponseDTO>();
        }

        List<Role> roles =
            await _roleRepository.GetAllAsync();

        List<Role> filteredRoles = roles
            .Where(r => r.DepartmentId == departmentId)
            .GroupBy(r => r.RoleName)
            .Select(g => g.First())
            .ToList();

        return _mapper.Map<List<RoleResponseDTO>>(filteredRoles);
    }


    public async Task<RoleResponseDTO> GetRoleByEmployeeIdAsync(int employeeId)
    {
        _logger.LogInformation(
            "Fetching role for EmployeeId {EmployeeId}.",
            employeeId);


        if (employeeId <= 0)
        {
            _logger.LogWarning(
                "Invalid EmployeeId {EmployeeId}.",
                employeeId);

            throw new InvalidFieldException(
                "Please provide valid EmployeeID.");
        }


        Role? role =
            await _roleRepository.GetRoleByEmployeeIdAsync(employeeId);


        if (role == null)
        {
            _logger.LogWarning(
                "No role found for EmployeeId {EmployeeId}.",
                employeeId);

            throw new ResourceNotFoundException(
                $"No Role Found for Employee Id {employeeId}.");
        }


        return _mapper.Map<RoleResponseDTO>(role);
    }


    public async Task<int> GetRoleIdByNameAsync(string roleName)
    {
        _logger.LogInformation(
            "Fetching RoleId for {RoleName}.",
            roleName);


        if (string.IsNullOrWhiteSpace(roleName))
        {
            _logger.LogWarning(
                "Invalid role name provided.");

            throw new InvalidFieldException(
                "Please provide valid role name.");
        }


        int roleId =
            await _roleRepository.GetRoleIdByNameAsync(roleName);


        if (roleId == 0)
        {
            _logger.LogWarning(
                "Role {RoleName} not found.",
                roleName);

            throw new ResourceNotFoundException(
                $"RoleId not found with role name {roleName}");
        }


        return roleId;
    }

    public async Task<RoleResponseDTO> GetRoleByNameAsync(string roleName)
    {
        _logger.LogInformation(
            "Fetching role details for role {RoleName}.",
            roleName);

        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new InvalidFieldException("Please provide a valid role name.");
        }

        var role = await _roleRepository.GetRoleByNameAsync(roleName);

        if (role == null)
        {
            throw new ResourceNotFoundException(
                $"Role '{roleName}' not found.");
        }

        return _mapper.Map<RoleResponseDTO>(role);
    }

    public async Task<List<RoleCardResponseDTO>> GetRoleCardsAsync()
    {
        List<Role> roles = await _roleRepository.GetRoleCardsAsync();

        return roles.Select(r => new RoleCardResponseDTO
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName,
            DepartmentName = r.Department?.DepartmentName ?? "N/A",
            LocationName = r.EmployeeRoles
                .Select(er => er.Employee?.Location?.LocationName)
                .FirstOrDefault() ?? "N/A",
            LocationId = r.EmployeeRoles
                .Select(er => er.Employee?.Location?.LocationId)
                .FirstOrDefault(),
            DepartmentId = r.DepartmentId,
            TotalEmployees = r.EmployeeRoles.Count
        }).ToList();
    
    }

    public async Task<bool> IsRoleExistsAsync(
        int departmentId,
        string roleName)
    {
        _logger.LogInformation(
            "Checking role existence for {RoleName} in DepartmentId {DepartmentId}.",
            roleName,
            departmentId);


        if (string.IsNullOrWhiteSpace(roleName))
        {
            _logger.LogWarning(
                "Invalid role name.");

            throw new InvalidFieldException(
                "Please provide valid role name.");
        }


        return await _roleRepository
            .IsRoleExistsAsync(departmentId, roleName);
    }
}
