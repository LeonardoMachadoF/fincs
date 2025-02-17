using FinCs.Domain.Entities;

namespace FinCs.Domain.Repositories.Expenses;

public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
    Task<List<Expense>> GetByMonth(DateOnly date);
}