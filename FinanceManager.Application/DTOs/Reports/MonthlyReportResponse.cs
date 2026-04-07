namespace FinanceManager.Application.DTOs.Reports;

public sealed class MonthlyReportResponse
{
    public Guid UserId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal Balance { get; set; }

    public IReadOnlyCollection<MonthlyReportItemResponse> Transactions { get; set; } = Array.Empty<MonthlyReportItemResponse>();
}
