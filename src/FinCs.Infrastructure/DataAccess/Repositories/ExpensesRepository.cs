using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(FinCsDbContext dbContext) : IExpensesRepository
{
    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<Expense?> GetById(long id)
    {
        return await dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}