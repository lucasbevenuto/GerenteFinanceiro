using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.DTOs.Transactions;

public sealed class TransactionResponse
{
    public Guid Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TransactionType Type { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
