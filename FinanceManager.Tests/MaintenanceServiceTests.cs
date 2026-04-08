using FinanceManager.Application.Services;
using FinanceManager.Tests.TestDoubles;

namespace FinanceManager.Tests;

public sealed class MaintenanceServiceTests
{
    [Fact]
    public async Task ClearAllAsync_ShouldInvokeRepositoryCleanup()
    {
        var repository = new InMemoryMaintenanceRepository();
        var service = new MaintenanceService(repository);

        await service.ClearAllAsync();

        Assert.True(repository.WasCleared);
    }
}
