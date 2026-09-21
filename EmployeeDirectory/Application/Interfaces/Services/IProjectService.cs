using Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;

namespace EmployeeDirectory.Application.Interfaces;

public interface IProjectService
{
    Task<List<string>> GetAllAsync();

    Task<List<ProjectResponseDTO>> GetAllProjectsAsync();

    Task<ProjectResponseDTO> GetProjectByEmployeeIdAsync(int employeeId);

    Task<int> GetProjectIdByNameAsync(string projectName);

    Task<ProjectResponseDTO?> GetProjectByIdAsync(int projectId);

    Task<ProjectResponseDTO> AddProjectAsync(ProjectRequestDTO request);

    Task<ProjectResponseDTO> UpdateProjectAsync(
        int projectId,
        ProjectRequestDTO request);

    Task DeleteProjectAsync(int projectId);
}

