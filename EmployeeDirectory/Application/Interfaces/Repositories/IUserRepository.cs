using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUserNameAsync(string userName);

    Task<User?> GetByEmployeeIdAsync(int employeeId);

    Task<bool> UserNameExistsAsync(string userName);

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}

