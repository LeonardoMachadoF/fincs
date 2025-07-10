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
}