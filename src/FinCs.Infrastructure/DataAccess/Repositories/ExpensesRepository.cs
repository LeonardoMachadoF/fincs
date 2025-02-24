using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(FinCsDbContext dbContext)
    : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    public async Task<List<Expense>> GetAll()
    {
        return await dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Expense>> GetByMonth(DateOnly date)
    {
        var startDate = new DateTime(date.Year, date.Month, 1).Date;

        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59);

        return await dbContext
            .Expenses
            .AsNoTracking()
            .Where(
                expense =>
                    expense.Date >= startDate
                    &&
                    expense.Date <= endDate
            )
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }

    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        if (result is null) return false;
        dbContext.Expenses.Remove(result);
        return true;
    }
}