using FinCs.Domain.Entities;

namespace FinCs.Domain.Repositories.Expenses;

public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll(Entities.User user);
    Task<Expense?> GetById(Entities.User user, long id);
    Task<List<Expense>> GetByMonth(Entities.User user, DateOnly date);
}