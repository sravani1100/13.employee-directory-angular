using Application.Interfaces.Repositories;
using Domain.Entities;
using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
    public class UserRepository : IUserRepository
    {
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
       .AsNoTracking()
       .Include(u => u.Employee)
       .Include(u => u.Role)
       .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User?> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.UserName == userName);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
       _context.Users.Update(user);
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
    }
}

