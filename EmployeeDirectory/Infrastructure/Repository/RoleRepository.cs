using EmployeeDirectory.Application.Repositories.Interfaces;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Infrastructure.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Role> AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        return role;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .OrderBy(r => r.RoleName)
            .ToListAsync();
    }

    public async Task<Role?> GetRoleByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeRoles
            .AsNoTracking()
            .Where(er => er.EmployeeId == employeeId)
            .Select(er => er.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetRoleIdByNameAsync(string roleName)
    {
        return await _context.Roles
            .AsNoTracking()
            .Where(r => r.RoleName == roleName)
            .Select(r => r.RoleId)
            .FirstOrDefaultAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles
            .Include(r => r.Department)
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<List<Role>> GetRoleCardsAsync()
    {
        return await _context.Roles
             .AsNoTracking()
             .Include(r => r.Department)
             .Include(r => r.EmployeeRoles)
                 .ThenInclude(er => er.Employee)
                     .ThenInclude(e => e.Location)
             .ToListAsync();
    }

    public async Task<bool> IsRoleExistsAsync(int departmentId, string roleName)
    {
        return await _context.Roles
            .AnyAsync(r => r.DepartmentId == departmentId &&
                      r.RoleName == roleName);
    }
}

