using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Repositories;

namespace EmployeeDirectory.Infrastructure.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;
    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderBy(p => p.ProjectName)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }


    public async Task<Project?> GetProjectByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeProjects
            .AsNoTracking()
            .Where(ep => ep.EmployeeId == employeeId)
            .Select(ep => ep.Project)
            .FirstOrDefaultAsync();
    }


    public async Task<int> GetProjectIdByNameAsync(string projectName)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.ProjectName == projectName)
            .Select(p => p.ProjectId)
            .FirstOrDefaultAsync();
    }


    public async Task<Project> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);

        await _context.SaveChangesAsync();

        return project;
    }


    public async Task<Project?> UpdateAsync(Project project)
    {
        Project? existingProject =
            await _context.Projects
                .FirstOrDefaultAsync(
                    p => p.ProjectId == project.ProjectId);


        if (existingProject == null)
        {
            return null;
        }


        existingProject.ProjectName = project.ProjectName;


        _context.Projects.Update(existingProject);

        await _context.SaveChangesAsync();


        return existingProject;
    }


    public async Task<bool> DeleteAsync(int projectId)
    {
        Project? project =
            await _context.Projects
                .FirstOrDefaultAsync(
                    p => p.ProjectId == projectId);


        if (project == null)
        {
            return false;
        }


        _context.Projects.Remove(project);

        await _context.SaveChangesAsync();


        return true;
    }
}

