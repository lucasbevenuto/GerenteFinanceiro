using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly FinanceDbContext _context;

    public CategoryRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Category>> GetAllAsync(
        TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .AsNoTracking()
            .AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(category => category.Type == type.Value);
        }

        return await query
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByNameAndTypeAsync(
        string name,
        TransactionType type,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(
                category => category.Type == type && category.Name.ToLower() == normalizedName,
                cancellationToken);
    }

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }
}
