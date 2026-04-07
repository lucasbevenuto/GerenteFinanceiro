using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Users;
using FinanceManager.Application.Interfaces.Persistence;
using FinanceManager.Application.Interfaces.Security;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Services;

public sealed class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IReadOnlyCollection<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users
            .Select(MapToResponse)
            .ToArray();
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User '{id}' was not found.");

        return MapToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["email"] = new[] { "The informed e-mail is already registered." }
            });
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        await _userRepository.AddAsync(user, cancellationToken);

        return MapToResponse(user);
    }

    private static void ValidateRequest(CreateUserRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = new[] { "Name is required." };
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors["email"] = new[] { "E-mail is required." };
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors["password"] = new[] { "Password is required." };
        }
        else if (request.Password.Length < 6)
        {
            errors["password"] = new[] { "Password must have at least 6 characters." };
        }

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
