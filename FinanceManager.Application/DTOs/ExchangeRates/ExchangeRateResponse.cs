namespace FinanceManager.Application.DTOs.ExchangeRates;

public sealed class ExchangeRateResponse
{
    public string BaseCurrency { get; set; } = string.Empty;

    public string TargetCurrency { get; set; } = string.Empty;

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }

    public decimal ConvertedAmount { get; set; }

    public DateTime QuotedAt { get; set; }

    public string Source { get; set; } = string.Empty;
}
