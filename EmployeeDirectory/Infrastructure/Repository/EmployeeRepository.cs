using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        return employee;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int? employeeId)
    {
        return await _context.Employees
       .AsNoTracking()

       .Include(e => e.Location)

       .Include(e => e.Manager)

       .Include(e => e.EmployeeRoles)
           .ThenInclude(er => er.Role)

       .Include(e => e.EmployeeProjects)
           .ThenInclude(ep => ep.Project)

       .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .AsNoTracking()

            .Include(e => e.EmployeeRoles)
                .ThenInclude(er => er.Role)
                    .ThenInclude(r => r.Department)

            .Include(e => e.Location)

            .Include(e => e.Manager)

            .Include(e => e.EmployeeProjects)
                .ThenInclude(ep => ep.Project)

            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByNumberAsync(string employeeNumber)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
    }

    public async Task<List<string>> GetAllManagersAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .Where(e => e.ManagerId == null)
            .Select(e => e.EmployeeId +" - "+ e.FirstName +" " +e.LastName)
            .ToListAsync();
    }

    public async Task<List<Employee>> GetEmployeesByManagerIdAsync(int managerId)
    {
        return await _context.Employees
            .Where(e => e.ManagerId == managerId)
            .ToListAsync();
    }

    public async Task<List<Employee>> GetEmployeeDetailsAsync()
    {
        return await _context.Employees
            .Include(e => e.Location)
            .Include(e => e.EmployeeRoles)
                .ThenInclude(er => er.Role)
                    .ThenInclude(r => r.Department)
            .Include(e => e.EmployeeProjects)
                .ThenInclude(ep => ep.Project)
            .Include(e => e.Manager)
            .AsNoTracking() 
            .ToListAsync();
    }
    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee> UpdateAsync(Employee updatedEmployee)
    {
        _context.Employees.Update(updatedEmployee);
        return updatedEmployee;
    }

    public async Task<bool> DeleteAsync(string employeeNumber)
    {
        Employee? employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

        if (employee == null)
            return false;

        _context.Employees.Remove(employee);
        return true;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email);
    }
}

