using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class EmployeeRoleRepository : IEmployeeRoleRepository
{
    private readonly AppDbContext _context;
    public EmployeeRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddEmployeeRoleAsync(EmployeeRole employeeRole)
    {
        await _context.EmployeeRoles.AddAsync(employeeRole);
    }

    public async Task DeleteByEmployeeIdAsync(int employeeId)
    {
        var roles = await _context.EmployeeRoles
            .Where(er => er.EmployeeId == employeeId)
            .ToListAsync();

        _context.EmployeeRoles.RemoveRange(roles);
    }

    public async Task<bool> IsExistsAsync(int employeeId, int roleId)
    {
        return await _context.EmployeeRoles
            .AnyAsync(er => er.EmployeeId == employeeId && er.RoleId == roleId);
    }
}

