using EmployeeDirectory.Infrastructure.DBConnection.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using EmployeeDirectory.Application.Repositories.Interfaces;

namespace EmployeeDirectory.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollBackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}

