using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.DTOs.Categories;

public sealed class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;

    public TransactionType Type { get; set; }
}
