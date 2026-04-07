using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.ExchangeRates;
using FinanceManager.Application.Interfaces.Services;

namespace FinanceManager.Application.Services;

public sealed class ExchangeRateAppService
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateAppService(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<ExchangeRateResponse> GetLatestAsync(
        string baseCurrency,
        string targetCurrency,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency) || string.IsNullOrWhiteSpace(targetCurrency))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["currency"] = new[] { "Base and target currency are required." }
            });
        }

        if (amount <= 0)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["amount"] = new[] { "Amount must be greater than zero." }
            });
        }

        var normalizedBase = baseCurrency.Trim().ToUpperInvariant();
        var normalizedTarget = targetCurrency.Trim().ToUpperInvariant();

        var (rate, quotedAt, source) = await _exchangeRateService.GetLatestRateAsync(
            normalizedBase,
            normalizedTarget,
            cancellationToken);

        return new ExchangeRateResponse
        {
            BaseCurrency = normalizedBase,
            TargetCurrency = normalizedTarget,
            Amount = amount,
            Rate = rate,
            ConvertedAmount = decimal.Round(amount * rate, 4),
            QuotedAt = quotedAt,
            Source = source
        };
    }
}
