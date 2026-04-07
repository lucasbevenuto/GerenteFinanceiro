using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Transactions;
using FinanceManager.Application.Services;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Tests.TestDoubles;

namespace FinanceManager.Tests;

public sealed class TransactionServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldRejectWhenCategoryTypeDoesNotMatchTransactionType()
    {
        var userRepository = new InMemoryUserRepository();
        var categoryRepository = new InMemoryCategoryRepository();
        var transactionRepository = new InMemoryTransactionRepository();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@email.com",
            PasswordHash = "hash"
        };

        var expenseCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Moradia",
            Type = TransactionType.Despesa
        };

        userRepository.Seed(user);
        categoryRepository.Seed(expenseCategory);

        var service = new TransactionService(transactionRepository, userRepository, categoryRepository);

        var action = () => service.CreateAsync(new CreateTransactionRequest
        {
            Description = "Salario",
            Amount = 3000,
            Date = new DateTime(2026, 4, 1),
            Type = TransactionType.Receita,
            UserId = user.Id,
            CategoryId = expenseCategory.Id
        });

        await Assert.ThrowsAsync<ValidationException>(action);
    }
}
