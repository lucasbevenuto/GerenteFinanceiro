namespace FinanceManager.Infrastructure.Integrations.ExchangeRates;

public sealed class ExchangeRateOptions
{
    public const string SectionName = "ExchangeRates";

    public string BaseUrl { get; set; } = "https://api.frankfurter.dev/v1/";
}
