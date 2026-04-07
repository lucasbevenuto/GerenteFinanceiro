using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Reports;
using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Services;

public sealed class ReportService
{
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;

    public ReportService(IUserRepository userRepository, ITransactionRepository transactionRepository)
    {
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<MonthlyReportResponse> GetMonthlyAsync(
        Guid userId,
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["userId"] = new[] { "UserId is required." }
            });
        }

        if (month is < 1 or > 12)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["month"] = new[] { "Month must be between 1 and 12." }
            });
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException($"User '{userId}' was not found.");

        var transactions = await _transactionRepository.GetByMonthAsync(userId, year, month, cancellationToken);
        var totalIncome = transactions
            .Where(t => t.Type == TransactionType.Receita)
            .Sum(t => t.Amount);
        var totalExpense = transactions
            .Where(t => t.Type == TransactionType.Despesa)
            .Sum(t => t.Amount);

        return new MonthlyReportResponse
        {
            UserId = user.Id,
            Year = year,
            Month = month,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            Balance = totalIncome - totalExpense,
            Transactions = transactions
                .OrderByDescending(t => t.Date)
                .Select(t => new MonthlyReportItemResponse
                {
                    TransactionId = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    Date = t.Date,
                    Type = t.Type,
                    CategoryName = t.Category?.Name ?? string.Empty
                })
                .ToArray()
        };
    }
}
