using Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "EmployeeRead")]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponseDTO>>> GetProjects()
    {
        List<ProjectResponseDTO> projects =
            await _projectService.GetAllProjectsAsync();

        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDTO>> GetProjectById(int id)
    {
        ProjectResponseDTO? project =
            await _projectService.GetProjectByIdAsync(id);

        return Ok(project);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ProjectResponseDTO>> GetProjectByEmployeeId(
        int employeeId)
    {
        ProjectResponseDTO project =
            await _projectService.GetProjectByEmployeeIdAsync(employeeId);

        return Ok(project);
    }

    [HttpGet("name/{projectName}")]
    public async Task<ActionResult<int>> GetProjectIdByName(string projectName)
    {
        int projectId =
            await _projectService.GetProjectIdByNameAsync(projectName);

        return Ok(projectId);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDTO>> AddProject(
        [FromBody] ProjectRequestDTO request)
    {
        ProjectResponseDTO project =
            await _projectService.AddProjectAsync(request);

        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = project.ProjectId },
            project);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectResponseDTO>> UpdateProject(
        int id,
        [FromBody] ProjectRequestDTO request)
    {
        ProjectResponseDTO project =
            await _projectService.UpdateProjectAsync(id, request);

        return Ok(project);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        await _projectService.DeleteProjectAsync(id);

        return NoContent();
    }
}

