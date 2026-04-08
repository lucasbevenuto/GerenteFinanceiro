using FinanceManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MaintenanceController : ControllerBase
{
    private readonly MaintenanceService _maintenanceService;

    public MaintenanceController(MaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpDelete("clear-all")]
    public async Task<IActionResult> ClearAll(CancellationToken cancellationToken)
    {
        await _maintenanceService.ClearAllAsync(cancellationToken);
        return NoContent();
    }
}
