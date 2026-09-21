using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "EmployeeRead")]
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult> GetDepartments()
    {

        List<string> departments = await _departmentService.GetAllAsync();
        return Ok(departments);

    }

    [HttpGet("Departments")]
    public async Task<ActionResult> GetAllDeaprtments()
    {
        List<DepartmentResponseDTO> departments = await _departmentService.GetAllDepartmentsAsync();

        return Ok(departments);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult> AddDepartment(
        DepartmentRequestDTO request)
    {

        DepartmentResponseDTO result = await _departmentService.AddDepartmentAsync(request);
        return Ok(result);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {

        DepartmentResponseDTO result = await _departmentService.GetDepartmentByIdAsync(id);
        return Ok(result);

    }
}

