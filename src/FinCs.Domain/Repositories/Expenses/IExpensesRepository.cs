using FinCs.Domain.Entities;

namespace FinCs.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
}