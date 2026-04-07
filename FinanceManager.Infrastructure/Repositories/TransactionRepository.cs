using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly FinanceDbContext _context;

    public TransactionRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(transaction => transaction.User)
            .Include(transaction => transaction.Category)
            .FirstOrDefaultAsync(transaction => transaction.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Transaction>> GetAllAsync(
        Guid? userId = null,
        int? year = null,
        int? month = null,
        TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery();

        if (userId.HasValue)
        {
            query = query.Where(transaction => transaction.UserId == userId.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(transaction => transaction.Date.Year == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(transaction => transaction.Date.Month == month.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(transaction => transaction.Type == type.Value);
        }

        return await query
            .OrderByDescending(transaction => transaction.Date)
            .ThenByDescending(transaction => transaction.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Transaction>> GetByMonthAsync(
        Guid userId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery()
            .Where(transaction =>
                transaction.UserId == userId &&
                transaction.Date.Year == year &&
                transaction.Date.Month == month)
            .OrderByDescending(transaction => transaction.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Transaction> AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction;
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Transaction> BuildQuery()
    {
        return _context.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.User)
            .Include(transaction => transaction.Category);
    }
}
