namespace FinanceManager.Application.Interfaces.Security;

public interface IPasswordHasher
{
    string Hash(string password);
}
