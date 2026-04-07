using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Transactions;
using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Services;

public sealed class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<TransactionResponse>> GetAllAsync(
        Guid? userId = null,
        int? year = null,
        int? month = null,
        TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        var transactions = await _transactionRepository.GetAllAsync(userId, year, month, type, cancellationToken);

        return transactions
            .Select(MapToResponse)
            .ToArray();
    }

    public async Task<TransactionResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Transaction '{id}' was not found.");

        return MapToResponse(transaction);
    }

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request.Description, request.Amount, request.Date, request.UserId, request.CategoryId);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException($"User '{request.UserId}' was not found.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException($"Category '{request.CategoryId}' was not found.");

        if (category.Type != request.Type)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["type"] = new[] { "Transaction type must match the selected category type." }
            });
        }

        var transaction = new Transaction
        {
            Description = request.Description.Trim(),
            Amount = request.Amount,
            Date = request.Date,
            Type = request.Type,
            UserId = user.Id,
            CategoryId = category.Id
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        transaction.User = user;
        transaction.Category = category;

        return MapToResponse(transaction);
    }

    public async Task<TransactionResponse> UpdateAsync(Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request.Description, request.Amount, request.Date, Guid.Empty, request.CategoryId, validateUserId: false);

        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Transaction '{id}' was not found.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException($"Category '{request.CategoryId}' was not found.");

        if (category.Type != request.Type)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["type"] = new[] { "Transaction type must match the selected category type." }
            });
        }

        transaction.Description = request.Description.Trim();
        transaction.Amount = request.Amount;
        transaction.Date = request.Date;
        transaction.Type = request.Type;
        transaction.CategoryId = category.Id;
        transaction.Category = category;

        await _transactionRepository.UpdateAsync(transaction, cancellationToken);

        return MapToResponse(transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Transaction '{id}' was not found.");

        await _transactionRepository.DeleteAsync(transaction, cancellationToken);
    }

    private static void ValidateRequest(
        string description,
        decimal amount,
        DateTime date,
        Guid userId,
        Guid categoryId,
        bool validateUserId = true)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(description))
        {
            errors["description"] = new[] { "Description is required." };
        }

        if (amount <= 0)
        {
            errors["amount"] = new[] { "Amount must be greater than zero." };
        }

        if (date == default)
        {
            errors["date"] = new[] { "Date is required." };
        }

        if (validateUserId && userId == Guid.Empty)
        {
            errors["userId"] = new[] { "UserId is required." };
        }

        if (categoryId == Guid.Empty)
        {
            errors["categoryId"] = new[] { "CategoryId is required." };
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }

    private static TransactionResponse MapToResponse(Transaction transaction)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type,
            UserId = transaction.UserId,
            UserName = transaction.User?.Name ?? string.Empty,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name ?? string.Empty,
            CreatedAt = transaction.CreatedAt
        };
    }
}
