using FinanceManager.Application.DTOs.Reports;
using FinanceManager.Application.Services;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Tests.TestDoubles;

namespace FinanceManager.Tests;

public sealed class ReportServiceTests
{
    [Fact]
    public async Task GetMonthlyAsync_ShouldCalculateIncomeExpenseAndBalance()
    {
        var userRepository = new InMemoryUserRepository();
        var transactionRepository = new InMemoryTransactionRepository();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Ana",
            Email = "ana@email.com",
            PasswordHash = "hash"
        };

        userRepository.Seed(user);

        transactionRepository.Seed(
            new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Description = "Salario",
                Amount = 5000,
                Date = new DateTime(2026, 4, 5),
                Type = TransactionType.Receita,
                Category = new Category { Id = Guid.NewGuid(), Name = "Trabalho", Type = TransactionType.Receita }
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Description = "Aluguel",
                Amount = 1800,
                Date = new DateTime(2026, 4, 8),
                Type = TransactionType.Despesa,
                Category = new Category { Id = Guid.NewGuid(), Name = "Moradia", Type = TransactionType.Despesa }
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Description = "Compra antiga",
                Amount = 50,
                Date = new DateTime(2026, 3, 20),
                Type = TransactionType.Despesa,
                Category = new Category { Id = Guid.NewGuid(), Name = "Lazer", Type = TransactionType.Despesa }
            });

        var service = new ReportService(userRepository, transactionRepository);

        MonthlyReportResponse report = await service.GetMonthlyAsync(user.Id, 2026, 4);

        Assert.Equal(5000, report.TotalIncome);
        Assert.Equal(1800, report.TotalExpense);
        Assert.Equal(3200, report.Balance);
        Assert.Equal(2, report.Transactions.Count);
    }
}
