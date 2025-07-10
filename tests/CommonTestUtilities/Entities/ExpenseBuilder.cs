using Bogus;
using FinCs.Domain.Entities;
using FinCs.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static Expense Build(User user)
    {
        var expense = new Faker<Expense>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Title, f => f.Commerce.ProductName())
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.Date, f => f.Date.Past())
            .RuleFor(r => r.Amount, f => f.Random.Decimal(1, 1000))
            .RuleFor(r => r.UserId, _ => user.Id)
            .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>());

        return expense;
    }

    public static List<Expense> Collection(User user, uint count = 2)
    {
        var list = new List<Expense>();
        if (count == 0) count = 1;

        var expenseId = 1;

        for (var i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = expenseId++;
            list.Add(expense);
        }

        return list;
    }
}