using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Application.Interfaces.Security;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Tests.TestDoubles;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public void Seed(params User[] users) => _users.AddRange(users);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.FirstOrDefault(user => user.Id == id));

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.FirstOrDefault(user => user.Email == email));

    public Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyCollection<User>>(_users.ToArray());

    public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id == Guid.Empty)
        {
            user.Id = Guid.NewGuid();
        }

        _users.Add(user);
        return Task.FromResult(user);
    }
}

internal sealed class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new();

    public void Seed(params Category[] categories) => _categories.AddRange(categories);

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_categories.FirstOrDefault(category => category.Id == id));

    public Task<IReadOnlyCollection<Category>> GetAllAsync(TransactionType? type = null, CancellationToken cancellationToken = default)
    {
        var result = type.HasValue
            ? _categories.Where(category => category.Type == type.Value).ToArray()
            : _categories.ToArray();

        return Task.FromResult<IReadOnlyCollection<Category>>(result);
    }

    public Task<Category?> GetByNameAndTypeAsync(string name, TransactionType type, CancellationToken cancellationToken = default)
        => Task.FromResult(
            _categories.FirstOrDefault(category =>
                category.Type == type &&
                string.Equals(category.Name, name, StringComparison.OrdinalIgnoreCase)));

    public Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (category.Id == Guid.Empty)
        {
            category.Id = Guid.NewGuid();
        }

        _categories.Add(category);
        return Task.FromResult(category);
    }
}

internal sealed class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public void Seed(params Transaction[] transactions) => _transactions.AddRange(transactions);

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_transactions.FirstOrDefault(transaction => transaction.Id == id));

    public Task<IReadOnlyCollection<Transaction>> GetAllAsync(
        Guid? userId = null,
        int? year = null,
        int? month = null,
        TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Transaction> query = _transactions;

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

        return Task.FromResult<IReadOnlyCollection<Transaction>>(query.ToArray());
    }

    public Task<IReadOnlyCollection<Transaction>> GetByMonthAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var result = _transactions
            .Where(transaction =>
                transaction.UserId == userId &&
                transaction.Date.Year == year &&
                transaction.Date.Month == month)
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<Transaction>>(result);
    }

    public Task<Transaction> AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction.Id == Guid.Empty)
        {
            transaction.Id = Guid.NewGuid();
        }

        _transactions.Add(transaction);
        return Task.FromResult(transaction);
    }

    public Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _transactions.Remove(transaction);
        return Task.CompletedTask;
    }
}

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => $"hashed::{password}";
}
