using FinCs.Domain.Entities;

namespace FinCs.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    Task Delete(long id);
}