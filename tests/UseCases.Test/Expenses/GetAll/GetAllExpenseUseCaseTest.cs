using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FinCs.Application.UseCases.Expenses.GetAll;
using FinCs.Domain.Entities;
using Shouldly;

namespace UseCases.Test.Expenses.GetAll;

public class GetAllExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Expenses.ShouldNotBeEmpty();

        foreach (var expense in expenses)
        {
            expense.Id.ShouldBeGreaterThan(0);
            expense.Title.ShouldNotBeNullOrEmpty();
            expense.Amount.ShouldBeGreaterThan(0);
        }
    }

    private GetAllExpenseUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new GetAllExpenseUseCase(loggedUser, repository, mapper);
    }
}