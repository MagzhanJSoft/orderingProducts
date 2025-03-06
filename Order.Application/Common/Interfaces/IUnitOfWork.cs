using Microsoft.EntityFrameworkCore.Storage;

namespace Order.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
