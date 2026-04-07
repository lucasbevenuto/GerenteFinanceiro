using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Interfaces.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Transaction>> GetAllAsync(
        Guid? userId = null,
        int? year = null,
        int? month = null,
        TransactionType? type = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Transaction>> GetByMonthAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default);

    Task<Transaction> AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
