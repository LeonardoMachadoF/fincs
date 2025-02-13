using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(FinCsDbContext dbContext) : IExpensesRepository
{
    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }
}