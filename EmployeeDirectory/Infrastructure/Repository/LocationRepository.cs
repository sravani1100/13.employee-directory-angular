using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Location>> GetAllAsync()
    {
        return await _context.Locations
            .AsNoTracking()
            .OrderBy(l => l.LocationName)
            .ToListAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(int locationId)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.LocationId == locationId);
    }

    public async Task<Location?> GetLocationByDepartmentIdAsync(int departmentId)
    {
        return await _context.LocationDepartments
            .AsNoTracking()
            .Where(ld => ld.DepartmentId == departmentId)
            .Select(ld => ld.Location)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetLocationIdByNameAsync(string locationName)
    {
        return await _context.Locations
            .AsNoTracking()
            .Where(l => l.LocationName == locationName)
            .Select(l => l.LocationId)
            .FirstOrDefaultAsync();
    }

    public async Task<Location> AddAsync(Location location)
    {
        await _context.Locations.AddAsync(location);

        await _context.SaveChangesAsync();

        return location;
    }

    public async Task<Location?> UpdateAsync(Location location)
    {
        Location? existingLocation =
            await _context.Locations
                .FirstOrDefaultAsync(l => l.LocationId == location.LocationId);

        if (existingLocation == null)
        {
            return null;
        }

        existingLocation.LocationName = location.LocationName;

        _context.Locations.Update(existingLocation);

        await _context.SaveChangesAsync();

        return existingLocation;
    }

    public async Task<bool> DeleteAsync(int locationId)
    {
        Location? location =
            await _context.Locations
                .FirstOrDefaultAsync(l => l.LocationId == locationId);

        if (location == null)
        {
            return false;
        }

        _context.Locations.Remove(location);

        await _context.SaveChangesAsync();

        return true;
    }
}


