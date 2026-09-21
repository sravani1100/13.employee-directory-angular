
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeDirectory.Application.Repositories.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollBackTransactionAsync();
}

