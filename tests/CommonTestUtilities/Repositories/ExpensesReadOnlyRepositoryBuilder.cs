using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository = new();

    public IExpensesReadOnlyRepository Build()
    {
        return _repository.Object;
    }

    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder GetByMonth(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);
        return this;
    }
}