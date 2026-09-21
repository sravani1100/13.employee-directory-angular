using AutoMapper;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectory.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDepartmentRepository departmentRepository,
        ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<DepartmentResponseDTO> AddDepartmentAsync(DepartmentRequestDTO request)
    {
        _logger.LogInformation("Adding new department {DepartmentName}.", request?.DepartmentName);

        if (request == null || string.IsNullOrWhiteSpace(request.DepartmentName))
        {
            _logger.LogWarning("Department creation failed due to invalid department name.");
            throw new InvalidFieldException("Cannot add Department. Give required department name to add.");
        }

        Department department = _mapper.Map<Department>(request);
        await _departmentRepository.AddAsync(department);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation( "Department {DepartmentName} created successfully with Id {DepartmentId}.",
            department.DepartmentName,
            department.DepartmentId);

        return _mapper.Map<DepartmentResponseDTO>(department);
    }


    public async Task<List<string>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all departments.");

        List<Department> departments = await _departmentRepository.GetAllAsync();

        _logger.LogInformation( "Retrieved {DepartmentCount} departments.",
            departments.Count);

        return departments
            .Select(d => d.DepartmentName)
            .Distinct()
            .ToList();
    }

    public async Task<List<DepartmentResponseDTO>> GetAllDepartmentsAsync()
    {
        List<Department> departments = await _departmentRepository.GetAllAsync();

        return _mapper.Map<List<DepartmentResponseDTO>>(departments);
    }

    public async Task<DepartmentResponseDTO> GetDepartmentByIdAsync(int departmentId)
    {
        _logger.LogInformation(
            "Fetching department by Id {DepartmentId}.",
            departmentId);

        if (departmentId <= 0)
        {
            _logger.LogWarning(
                "Invalid department Id {DepartmentId}.",
                departmentId);

            throw new InvalidFieldException("Please provide valid department Id.");
        }

        Department? department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);

        if (department == null)
        {
            _logger.LogWarning(
                "Department with Id {DepartmentId} not found.",
                departmentId);

            throw new ResourceNotFoundException("Department not found.");
        }

        _logger.LogInformation(
            "Department {DepartmentId} retrieved successfully.",
            departmentId);

        return _mapper.Map<DepartmentResponseDTO>(department);
    }

    public async Task<List<string>> GetDepartmentsAsync()
    {
        _logger.LogInformation("Fetching department names excluding Other department.");

        List<Department> departments = await _departmentRepository.GetAllAsync();

        if (departments.Count == 0)
        {
            _logger.LogWarning("No departments found.");
            throw new ResourceNotFoundException("No department found.");
        }

        return departments
            .Where(department => department.DepartmentName != "Other")
            .Select(d => d.DepartmentName)
            .Distinct()
            .ToList();
    }


    public async Task<DepartmentResponseDTO> GetDepartmentByLocationIdAsync(int locationId)
    {
        _logger.LogInformation(
            "Fetching department by LocationId {LocationId}.",
            locationId);

        if (locationId <= 0)
        {
            _logger.LogWarning("Invalid LocationId {LocationId}.",
                locationId);
            throw new InvalidFieldException("Please provide valid locationId.");
        }

        Department? department = await _departmentRepository.GetDepartmentByLocationIdAsync(locationId);

        if (department == null)
        {
            _logger.LogWarning(
                "No department found for LocationId {LocationId}.",
                locationId);

            throw new ResourceNotFoundException($"Department with Location Id {locationId} not found.");
        }

        return _mapper.Map<DepartmentResponseDTO>(department);
    }


    public async Task<int> GetDepartmentIdByNameAsync(string departmentName)
    {
        _logger.LogInformation(
            "Fetching department Id for {DepartmentName}.",
            departmentName);

        if (string.IsNullOrWhiteSpace(departmentName))
        {
            _logger.LogWarning(
                "Invalid department name provided.");

            throw new InvalidFieldException("Please provide valid department name.");
        }

        int departmentId = await _departmentRepository.GetDepartmentIdByNameAsync(departmentName);

        if (departmentId == 0)
            _logger.LogWarning("Department {DepartmentName} does not exist.", departmentName);        

        return departmentId;
    }
}

