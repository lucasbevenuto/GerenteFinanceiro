using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Application.DTOs.Users;
using FinanceManager.Application.Services;
using FinanceManager.Domain.Entities;
using FinanceManager.Tests.TestDoubles;

namespace FinanceManager.Tests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldHashPasswordAndPersistUser()
    {
        var repository = new InMemoryUserRepository();
        var service = new UserService(repository, new FakePasswordHasher());

        var result = await service.CreateAsync(new CreateUserRequest
        {
            Name = "Maria",
            Email = "maria@email.com",
            Password = "123456"
        });

        var storedUser = await repository.GetByEmailAsync("maria@email.com");

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotNull(storedUser);
        Assert.Equal("hashed::123456", storedUser!.PasswordHash);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectDuplicateEmail()
    {
        var repository = new InMemoryUserRepository();
        repository.Seed(new User
        {
            Id = Guid.NewGuid(),
            Name = "Maria",
            Email = "maria@email.com",
            PasswordHash = "already-hashed"
        });

        var service = new UserService(repository, new FakePasswordHasher());

        var action = () => service.CreateAsync(new CreateUserRequest
        {
            Name = "Outra Maria",
            Email = "maria@email.com",
            Password = "123456"
        });

        await Assert.ThrowsAsync<ValidationException>(action);
    }
}
