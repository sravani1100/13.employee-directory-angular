using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class LocationDepartmentRepository : ILocationDepartmentRepository
{
    private readonly AppDbContext _context;
    public LocationDepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddLocationDepartmentAsync(LocationDepartment locationDepartment)
    {
        await _context.LocationDepartments.AddAsync(locationDepartment);
    }

    public async Task<List<LocationDepartment>> GetAllAsync()
    {
        return await _context
            .LocationDepartments
            .AsNoTracking()
            .OrderBy(ld => ld.LocationDepartmentId)
            .ToListAsync();
    }

    public async Task<bool> IsExistsAsync(int locationId, int departmentId)
    {
        return await _context.LocationDepartments
            .AnyAsync(ld => ld.LocationId == locationId && ld.DepartmentId == departmentId);
    }
}

