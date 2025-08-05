using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FinCs.Application.UseCases.Expenses.Delete;
using FinCs.Domain.Entities;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_ExpenseNotFound()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }


    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repositoryReadOnly = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(
            repositoryReadOnly, repositoryWriteOnly, unityOfWork, loggedUser
        );
    }
}