using Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "EmployeeRead")]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(
        IEmployeeService employeeService,
        ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDTO>> Create([FromBody] EmployeeRequestDTO employee)
    {
        try
        {
            _logger.LogInformation("Create employee request received.");

            EmployeeResponseDTO createdEmployee = await _employeeService.AddEmployeeAsync(employee);


            _logger.LogInformation(
                "Employee created successfully. EmployeeNumber {EmployeeNumber}.",
                createdEmployee.EmployeeNumber);


            return CreatedAtAction(
                nameof(GetByEmployeeNumber),
                new
                {
                    employeeNumber = createdEmployee.EmployeeNumber
                },
                createdEmployee);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Employee creation failed. Resource not found: {Message}.",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Employee creation failed. Invalid data: {Message}.",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponseDTO>>> GetAll()
    {
        try
        {
            _logger.LogInformation(
                "Get all employees request received.");


            List<EmployeeResponseDTO> employees =
                await _employeeService.GetAllEmployeesAsync();


            _logger.LogInformation(
                "Returning {EmployeeCount} employees.",
                employees.Count);


            return Ok(employees);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "No employees found. {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid request while fetching employees. {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet("id/{id:int}")]
    public async Task<ActionResult<EmployeeDetailsResponseDTO>> GetById(int id)
    {
        try
        {
            _logger.LogInformation(
                "Fetching employee by Id {EmployeeId}.",
                id);


            EmployeeDetailsResponseDTO employee =
                await _employeeService.GetEmployeeByIdAsync(id);


            return Ok(employee);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Employee {EmployeeId} not found.",
                id);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid employee Id {EmployeeId}.",
                id);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet("{employeeNumber}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetByEmployeeNumber(
        string employeeNumber)
    {
        try
        {
            _logger.LogInformation(
                "Fetching employee {EmployeeNumber}.",
                employeeNumber);


            EmployeeResponseDTO employee =
                await _employeeService.GetEmployeeByNumberAsync(employeeNumber);


            return Ok(employee);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Employee {EmployeeNumber} not found.",
                employeeNumber);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid employee number {EmployeeNumber}.",
                employeeNumber);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet("managers")]
    public async Task<ActionResult<List<string>>> GetManagers()
    {
        try
        {
            _logger.LogInformation(
                "Fetching managers list.");


            List<string> managers =
                await _employeeService.GetAllManagersAsync();


            return Ok(managers);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Managers not found. {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid managers request. {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{employeeNumber}")]
    public async Task<ActionResult<EmployeeResponseDTO>> Update(
        string employeeNumber,
        [FromBody] EmployeeRequestDTO employee)
    {
        try
        {
            _logger.LogInformation(
                "Updating employee {EmployeeNumber}.",
                employeeNumber);


            EmployeeDetailsResponseDTO updatedEmployee =
                await _employeeService
                .UpdateEmployeeByNumberAsync(employeeNumber, employee);


            return Ok(updatedEmployee);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Employee {EmployeeNumber} update failed.",
                employeeNumber);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid update request for employee {EmployeeNumber}.",
                employeeNumber);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{employeeNumber}")]
    public async Task<IActionResult> Delete(string employeeNumber)
    {
        try
        {
            _logger.LogInformation(
                "Deleting employee {EmployeeNumber}.",
                employeeNumber);

            bool isDeleted =
                await _employeeService
                .DeleteEmployeeByNumberAsync(employeeNumber);

            if (!isDeleted)
            {
                _logger.LogWarning(
                    "Employee {EmployeeNumber} not found for deletion.",
                    employeeNumber);

                return NotFound(
                    $"Employee with number {employeeNumber} not found.");
            }
            _logger.LogInformation(
                "Employee {EmployeeNumber} deleted successfully.",
                employeeNumber);

            return NoContent();
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Employee deletion failed. {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid employee delete request. {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
