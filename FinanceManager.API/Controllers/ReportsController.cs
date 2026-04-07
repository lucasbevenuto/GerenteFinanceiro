using FinanceManager.Application.DTOs.Reports;
using FinanceManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportsController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<MonthlyReportResponse>> GetMonthly(
        [FromQuery] Guid userId,
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken cancellationToken)
    {
        var report = await _reportService.GetMonthlyAsync(userId, year, month, cancellationToken);
        return Ok(report);
    }
}
