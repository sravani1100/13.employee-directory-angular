using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;
    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Department> AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        return department;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();
    }

    public async Task<Department?> GetDepartmentByLocationIdAsync(int locationId)
    {
        return await _context.LocationDepartments
            .AsNoTracking()
            .Where(ld => ld.LocationId == locationId)
            .Select(ld => ld.Department)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetDepartmentIdByNameAsync(string departmentName)
    {
        return await _context.Departments
            .AsNoTracking()
            .Where(d => d.DepartmentName == departmentName)
            .Select(d => d.DepartmentId)
            .FirstOrDefaultAsync();
    }

    public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
    {
        return await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
    }
}
