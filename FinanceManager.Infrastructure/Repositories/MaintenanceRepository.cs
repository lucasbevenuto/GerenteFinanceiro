using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public sealed class MaintenanceRepository : IMaintenanceRepository
{
    private readonly FinanceDbContext _context;

    public MaintenanceRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        await _context.Transactions.ExecuteDeleteAsync(cancellationToken);
        await _context.Categories.ExecuteDeleteAsync(cancellationToken);
        await _context.Users.ExecuteDeleteAsync(cancellationToken);
    }
}
