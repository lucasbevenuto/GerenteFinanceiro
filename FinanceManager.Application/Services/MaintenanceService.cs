using FinanceManager.Application.Interfaces.Persistence;

namespace FinanceManager.Application.Services;

public sealed class MaintenanceService
{
    private readonly IMaintenanceRepository _maintenanceRepository;

    public MaintenanceService(IMaintenanceRepository maintenanceRepository)
    {
        _maintenanceRepository = maintenanceRepository;
    }

    public Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        return _maintenanceRepository.ClearAllAsync(cancellationToken);
    }
}
