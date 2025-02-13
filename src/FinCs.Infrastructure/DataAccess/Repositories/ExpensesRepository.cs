using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(FinCsDbContext dbContext) : IExpensesRepository
{
    public void Add(Expense expense)
    {
        dbContext.Expenses.Add(expense);
    }
}