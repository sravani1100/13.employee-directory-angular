using EmployeeDirectory.Domain.Entities;

namespace EmployeeDirectory.Application.Repositories.Repositories;

public interface IProjectRepository
{
   Task<List<Project>> GetAllAsync();

    Task<Project?> GetProjectByEmployeeIdAsync(int employeeId);

    Task<int> GetProjectIdByNameAsync(string projectName);

    Task<Project> AddAsync(Project project);

    Task<Project?> UpdateAsync(Project project);

    Task<bool> DeleteAsync(int projectId);

    Task<Project?> GetByIdAsync(int projectId);
}

