using FinCs.Communication.Requests;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.User;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FinCs.Application.UseCases.Users.Update;

public class UpdateUserUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork)
    : IUpdateUserUseCase
{
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser1 = await loggedUser.Get();

        await Validate(request, loggedUser1.Email);

        var user = await repository.GetById(loggedUser1.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}