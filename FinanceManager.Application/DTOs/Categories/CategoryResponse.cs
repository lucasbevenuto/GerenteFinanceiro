using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.DTOs.Categories;

public sealed class CategoryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public DateTime CreatedAt { get; set; }
}
