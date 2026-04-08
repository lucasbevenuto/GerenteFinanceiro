namespace FinanceManager.Application.Interfaces.Persistence;

public interface IMaintenanceRepository
{
    Task ClearAllAsync(CancellationToken cancellationToken = default);
}
