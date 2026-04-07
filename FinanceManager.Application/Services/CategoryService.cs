using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Categories;
using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Services;

public sealed class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<CategoryResponse>> GetAllAsync(
        TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(type, cancellationToken);

        return categories
            .Select(MapToResponse)
            .ToArray();
    }

    public async Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Category '{id}' was not found.");

        return MapToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var existingCategory = await _categoryRepository.GetByNameAndTypeAsync(request.Name.Trim(), request.Type, cancellationToken);
        if (existingCategory is not null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["name"] = new[] { "A category with the same name and type already exists." }
            });
        }

        var category = new Category
        {
            Name = request.Name.Trim(),
            Type = request.Type
        };

        await _categoryRepository.AddAsync(category, cancellationToken);

        return MapToResponse(category);
    }

    private static void ValidateRequest(CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["name"] = new[] { "Name is required." }
            });
        }
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
            CreatedAt = category.CreatedAt
        };
    }
}
