using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FinCs.Application.UseCases.Expenses.GetById;
using FinCs.Communication.Enums;
using FinCs.Domain.Entities;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Expenses.GetById;

public class GetExpenseByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var result = await useCase.Execute(expense.Id);

        result.ShouldNotBeNull();

        result.Id.ShouldBe(expense.Id);
        result.Title.ShouldBe(expense.Title);
        result.Description.ShouldBe(expense.Description);
        result.Date.ShouldBe(expense.Date);
        result.Amount.ShouldBe(expense.Amount);
        result.Tags.ShouldNotBeNull();
        result.Tags.ShouldNotBeEmpty();
        result.PaymentType.ShouldBe((PaymentType)expense.PaymentType);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);
        var act = async () => await useCase.Execute(1000);
        var result = await act().ShouldThrowAsync<NotFoundException>();

        result.GetErrors().ShouldHaveSingleItem()
            .ShouldBe(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }

    private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new GetExpenseByIdUseCase(loggedUser, repository, mapper);
    }
}