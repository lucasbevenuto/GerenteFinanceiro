namespace FinanceManager.Application.Interfaces.Services;

public interface IExchangeRateService
{
    Task<(decimal Rate, DateTime QuotedAt, string Source)> GetLatestRateAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default);
}
