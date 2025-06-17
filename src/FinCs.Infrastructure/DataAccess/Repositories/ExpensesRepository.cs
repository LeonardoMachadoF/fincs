using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(FinCsDbContext dbContext)
    : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    public async Task<List<Expense>> GetAll(User user)
    {
        return await dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
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

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id && x.Id == user.Id);
    }

    public void Update(Expense expense)
    {
        dbContext.Expenses.Update(expense);
    }

    public async Task Add(Expense expense)
    {
        await dbContext.Expenses.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await dbContext.Expenses.FirstAsync(x => x.Id == id);

        dbContext.Expenses.Remove(result);
    }
}