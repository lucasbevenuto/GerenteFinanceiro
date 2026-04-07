using FinanceManager.Application.DTOs.ExchangeRates;
using FinanceManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ExchangeRatesController : ControllerBase
{
    private readonly ExchangeRateAppService _exchangeRateAppService;

    public ExchangeRatesController(ExchangeRateAppService exchangeRateAppService)
    {
        _exchangeRateAppService = exchangeRateAppService;
    }

    [HttpGet("latest")]
    public async Task<ActionResult<ExchangeRateResponse>> GetLatest(
        [FromQuery] string baseCurrency,
        [FromQuery] string targetCurrency,
        [FromQuery] decimal amount = 1,
        CancellationToken cancellationToken = default)
    {
        var response = await _exchangeRateAppService.GetLatestAsync(
            baseCurrency,
            targetCurrency,
            amount,
            cancellationToken);

        return Ok(response);
    }
}
