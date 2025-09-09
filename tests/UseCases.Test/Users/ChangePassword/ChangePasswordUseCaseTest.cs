using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinCs.Application.UseCases.Users.ChangePassword;
using FinCs.Domain.Entities;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.INVALID_PASSWORD);
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
    }


    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncrypterBuilder()
            .Verify(password)
            .Build();


        return new ChangePasswordUseCase(
            loggedUser,
            passwordEncripter,
            userUpdateRepository,
            unitOfWork
        );
    }
}