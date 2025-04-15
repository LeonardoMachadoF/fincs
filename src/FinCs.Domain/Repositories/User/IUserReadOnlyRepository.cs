namespace FinCs.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    Task<Entities.User?> GetActiveUserWithEmail(string email);
}