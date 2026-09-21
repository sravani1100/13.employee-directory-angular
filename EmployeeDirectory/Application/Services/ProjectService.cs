using Application.DTO.RequestDTOs;
using AutoMapper;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Application.Repositories.Repositories;
using EmployeeDirectory.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectory.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(
        IMapper mapper,
        IProjectRepository projectRepository,
        ILogger<ProjectService> logger)
    {
        _mapper = mapper;
        _projectRepository = projectRepository;
        _logger = logger;
    }


    public async Task<ProjectResponseDTO?> GetProjectByIdAsync(int projectId)
    {
        _logger.LogInformation(
            "Fetching project with Id {ProjectId}.",
            projectId);

        if (projectId <= 0)
        {
            throw new InvalidFieldException(
                "Please provide valid project id.");
        }

        Project? project =
            await _projectRepository.GetByIdAsync(projectId);

        if (project == null)
        {
            throw new ResourceNotFoundException(
                $"Project with Id {projectId} not found.");
        }

        return _mapper.Map<ProjectResponseDTO>(project);
    }

    public async Task<List<string>> GetAllAsync()
    {
        _logger.LogInformation(
            "Fetching all projects.");


        List<Project> projects =
            await _projectRepository.GetAllAsync();


        if (projects.Count == 0)
        {
            _logger.LogWarning(
                "No projects found.");

            throw new ResourceNotFoundException(
                "No Project Found.");
        }


        _logger.LogInformation(
            "Retrieved {ProjectCount} projects.",
            projects.Count);


        return projects
            .Select(p => p.ProjectName)
            .ToList();
    }

    public async Task<List<ProjectResponseDTO>> GetAllProjectsAsync()
    {
        List<Project> projects = await _projectRepository.GetAllAsync();

        return _mapper.Map<List<ProjectResponseDTO>>(projects);
    }

    public async Task<ProjectResponseDTO> GetProjectByEmployeeIdAsync(int employeeId)
    {
        _logger.LogInformation(
            "Fetching project for EmployeeId {EmployeeId}.",
            employeeId);


        if (employeeId <= 0)
        {
            _logger.LogWarning(
                "Invalid EmployeeId {EmployeeId}.",
                employeeId);

            throw new InvalidFieldException(
                "Please provide valid EmployeeId.");
        }


        Project? project =
            await _projectRepository.GetProjectByEmployeeIdAsync(employeeId);


        if (project == null)
        {
            _logger.LogWarning(
                "No project found for EmployeeId {EmployeeId}.",
                employeeId);

            throw new ResourceNotFoundException(
                $"No Project Found For Employee Id {employeeId}.");
        }


        _logger.LogInformation(
            "Project {ProjectName} found for EmployeeId {EmployeeId}.",
            project.ProjectName,
            employeeId);


        return _mapper.Map<ProjectResponseDTO>(project);
    }


    public async Task<int> GetProjectIdByNameAsync(string projectName)
    {
        _logger.LogInformation(
            "Fetching project Id for ProjectName {ProjectName}.",
            projectName);


        if (string.IsNullOrWhiteSpace(projectName))
        {
            _logger.LogWarning(
                "Invalid project name provided.");

            throw new InvalidFieldException(
                "Please provide valid project name.");
        }


        int projectId =
            await _projectRepository.GetProjectIdByNameAsync(projectName);


        if (projectId == 0)
        {
            _logger.LogWarning(
                "Project not found with name {ProjectName}.",
                projectName);

            throw new ResourceNotFoundException(
                $"ProjectId not found with project name {projectName}");
        }


        _logger.LogInformation(
            "Project {ProjectName} found with Id {ProjectId}.",
            projectName,
            projectId);


        return projectId;
    }

    public async Task<ProjectResponseDTO> AddProjectAsync(
    ProjectRequestDTO request)
    {
        _logger.LogInformation(
            "Adding new project {ProjectName}.",
            request.ProjectName);

        if (string.IsNullOrWhiteSpace(request.ProjectName))
        {
            throw new InvalidFieldException(
                "Project name is required.");
        }

        int existingProjectId =
            await _projectRepository.GetProjectIdByNameAsync(
                request.ProjectName);

        if (existingProjectId != 0)
        {
            throw new InvalidFieldException(
                "Project already exists.");
        }

        Project project =
            _mapper.Map<Project>(request);

        Project addedProject =
            await _projectRepository.AddAsync(project);

        _logger.LogInformation(
            "Project {ProjectName} added successfully.",
            addedProject.ProjectName);

        return _mapper.Map<ProjectResponseDTO>(addedProject);
    }

    public async Task<ProjectResponseDTO> UpdateProjectAsync(
    int projectId,
    ProjectRequestDTO request)
    {
        _logger.LogInformation(
            "Updating project {ProjectId}.",
            projectId);

        if (projectId <= 0)
        {
            throw new InvalidFieldException(
                "Invalid project id.");
        }

        if (string.IsNullOrWhiteSpace(request.ProjectName))
        {
            throw new InvalidFieldException(
                "Project name is required.");
        }

        Project? existingProject =
            await _projectRepository.GetByIdAsync(projectId);

        if (existingProject == null)
        {
            throw new ResourceNotFoundException(
                $"Project {projectId} not found.");
        }

        _mapper.Map(request, existingProject);

        Project? updatedProject =
            await _projectRepository.UpdateAsync(existingProject);

        _logger.LogInformation(
            "Project {ProjectId} updated successfully.",
            projectId);

        return _mapper.Map<ProjectResponseDTO>(updatedProject);
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        _logger.LogInformation(
            "Deleting project {ProjectId}.",
            projectId);

        if (projectId <= 0)
        {
            throw new InvalidFieldException(
                "Invalid project id.");
        }

        Project? project =
            await _projectRepository.GetByIdAsync(projectId);

        if (project == null)
        {
            throw new ResourceNotFoundException(
                $"Project {projectId} not found.");
        }

        bool deleted =
            await _projectRepository.DeleteAsync(projectId);

        if (!deleted)
        {
            throw new ResourceNotFoundException(
                $"Unable to delete project {projectId}.");
        }

        _logger.LogInformation(
            "Project {ProjectId} deleted successfully.",
            projectId);
    }
}
