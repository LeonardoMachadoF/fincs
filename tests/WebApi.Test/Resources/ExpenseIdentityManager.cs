using FinCs.Domain.Entities;

namespace WebApi.Test.Resources;

public class ExpenseIdentityManager
{
    private readonly Expense _expense;

    public ExpenseIdentityManager(Expense expense)
    {
        _expense = expense;
    }

    public long GetId()
    {
        return _expense.Id;
    }

    public DateTime GetDate()
    {
        return _expense.Date;
    }
}