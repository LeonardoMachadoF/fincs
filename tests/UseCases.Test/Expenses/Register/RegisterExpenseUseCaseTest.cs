using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinCs.Application.UseCases.Expenses.Register;
using FinCs.Domain.Entities;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
            useCase.Execute(request));

        var errors = exception.GetErrors();
        errors.Count.ShouldBe(1);
        errors.ShouldContain(ResourceErrorMessages.TITLE_REQUIRED);
    }


    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}