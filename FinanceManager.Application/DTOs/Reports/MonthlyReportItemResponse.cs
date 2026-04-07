using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.DTOs.Reports;

public sealed class MonthlyReportItemResponse
{
    public Guid TransactionId { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TransactionType Type { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}
