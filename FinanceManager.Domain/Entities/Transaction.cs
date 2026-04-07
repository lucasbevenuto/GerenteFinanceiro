using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class Transaction : BaseEntity
{
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TransactionType Type { get; set; }

    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public User? User { get; set; }

    public Category? Category { get; set; }
}
