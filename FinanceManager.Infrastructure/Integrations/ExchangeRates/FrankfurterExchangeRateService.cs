using System.Net.Http.Json;
using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.Interfaces.Services;

namespace FinanceManager.Infrastructure.Integrations.ExchangeRates;

public sealed class FrankfurterExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public FrankfurterExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(decimal Rate, DateTime QuotedAt, string Source)> GetLatestRateAsync(
        string baseCurrency,
        string targetCurrency,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<FrankfurterResponse>(
            $"latest?base={baseCurrency}&symbols={targetCurrency}",
            cancellationToken);

        if (response?.Rates is null || !response.Rates.TryGetValue(targetCurrency, out var rate))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["currency"] = new[] { "Unable to retrieve quotation for the informed currency pair." }
            });
        }

        var quotedAt = DateTime.TryParse(response.Date, out var parsedDate)
            ? parsedDate
            : DateTime.UtcNow;

        return (rate, quotedAt, "Frankfurter");
    }

    private sealed class FrankfurterResponse
    {
        public string Base { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}
