using FinCs.Communication.Requests;
using FinCs.Domain.Entities;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.User;
using FinCs.Domain.Security.Cryptography;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FinCs.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase(
    ILoggedUser loggedUser,
    IPasswordEncripter passwordEncripter,
    IUserUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork)
    : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePasswordJson request)
    {
        var cLoggedUser = await loggedUser.Get();
        Validate(request, cLoggedUser);

        var user = await repository.GetById(cLoggedUser.Id);
        user.Password = passwordEncripter.Encript(request.NewPassword);

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, User cLoggedUser)
    {
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        var passwordMatch = passwordEncripter.Verify(request.Password, cLoggedUser.Password);

        if (passwordMatch == false)
            result.Errors.Add(new ValidationFailure(string.Empty,
                ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (result.IsValid) return;
        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errors);
    }
}