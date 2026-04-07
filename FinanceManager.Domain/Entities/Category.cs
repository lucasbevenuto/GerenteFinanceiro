using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
