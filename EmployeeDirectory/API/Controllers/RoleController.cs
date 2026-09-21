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
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(
        IRoleService roleService,
        ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RoleResponseDTO>> Create(
        [FromBody] RoleRequestDTO request)
    {
        try
        {
            _logger.LogInformation(
                "Create role request received for role {RoleName}.",
                request?.RoleName);


            RoleResponseDTO role =
                await _roleService.AddRoleAsync(request);


            _logger.LogInformation(
                "Role {RoleName} created successfully with Id {RoleId}.",
                role.RoleName,
                role.RoleId);


            return CreatedAtAction(
                nameof(GetRoleIdByName),
                new
                {
                    roleName = role.RoleName
                },
                role);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Role creation failed. Resource not found: {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Role creation failed. Invalid data: {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet]
    public async Task<ActionResult<List<RoleResponseDTO>>> GetAll()
    {
        try
        {
            _logger.LogInformation(
                "Fetching all roles.");


            List<RoleResponseDTO> roles =
                await _roleService.GetAllRolesAsync();


            _logger.LogInformation(
                "Returning {RoleCount} roles.",
                roles.Count);


            return Ok(roles);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "No roles found. {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid role request. {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<RoleResponseDTO>> GetByEmployeeId(
        int employeeId)
    {
        try
        {
            _logger.LogInformation(
                "Fetching role for employee {EmployeeId}.",
                employeeId);


            RoleResponseDTO role =
                await _roleService.GetRoleByEmployeeIdAsync(employeeId);


            return Ok(role);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Role not found for employee {EmployeeId}.",
                employeeId);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid employee id {EmployeeId}.",
                employeeId);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpGet("id")]
    public async Task<ActionResult<int>> GetRoleIdByName(
        [FromQuery] string roleName)
    {
        try
        {
            _logger.LogInformation(
                "Fetching role id for role {RoleName}.",
                roleName);


            int roleId =
                await _roleService.GetRoleIdByNameAsync(roleName);


            return Ok(roleId);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Role {RoleName} not found.",
                roleName);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid role name {RoleName}.",
                roleName);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("cards")]
    public async Task<ActionResult<List<RoleCardResponseDTO>>> GetRoleCards()
    {
        try
        {
            _logger.LogInformation("Fetching role cards.");

            List<RoleCardResponseDTO> roles =
                await _roleService.GetRoleCardsAsync();

            return Ok(roles);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Role cards not found: {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid role card request: {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<RoleResponseDTO>>> GetRolesByDepartmentAndLocation(
        [FromQuery] int departmentId,
        [FromQuery] int locationId)
    {
        try
        {
            _logger.LogInformation(
                "Fetching roles for DepartmentId {DepartmentId} and LocationId {LocationId}.",
                departmentId,
                locationId);


            List<RoleResponseDTO> roles =
                await _roleService
                .GetRolesByDepartmentAndLocationAsync(
                    departmentId,
                    locationId);


            if (roles == null || roles.Count == 0)
            {
                _logger.LogWarning(
                    "No roles found for DepartmentId {DepartmentId} and LocationId {LocationId}.",
                    departmentId,
                    locationId);

                return NotFound(
                    "No roles found for the given department and location.");
            }


            _logger.LogInformation(
                "Returning {RoleCount} roles.",
                roles.Count);


            return Ok(roles);
        }
        catch (ResourceNotFoundException ex)
        {
            _logger.LogWarning(
                "Role filter failed. {Message}",
                ex.Message);

            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            _logger.LogWarning(
                "Invalid role filter request. {Message}",
                ex.Message);

            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("name")]
    public async Task<ActionResult<RoleResponseDTO>> GetRoleByName(
    [FromQuery] string roleName)
    {
        try
        {
            var role = await _roleService.GetRoleByNameAsync(roleName);

            return Ok(role);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidFieldException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
