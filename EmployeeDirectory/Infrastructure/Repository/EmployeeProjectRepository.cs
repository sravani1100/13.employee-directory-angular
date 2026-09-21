using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class EmployeeProjectRepository : IEmployeeProjectRepository
{
    private readonly AppDbContext _context;

    public EmployeeProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddEmployeeProjectAsync(EmployeeProject employeeProject)
    {
        await _context.EmployeeProjects.AddAsync(employeeProject);
    }

    public async Task DeleteByEmployeeIdAsync(int employeeId)
    {
        var projects = await _context.EmployeeProjects
            .Where(ep => ep.EmployeeId == employeeId)
            .ToListAsync();

        _context.EmployeeProjects.RemoveRange(projects);
    }

    public async Task<bool> IsExistsAsync(int employeeId, int projectId)
    {
        return await _context.EmployeeProjects
            .AnyAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);
    }
}

