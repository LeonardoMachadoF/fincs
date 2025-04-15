using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Repositories.User;
using FinCs.Domain.Security.Cryptography;
using FinCs.Domain.Security.Tokens;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Users.Login;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IPasswordEncripter passwordEncripter,
    IAccessTokenGenerator accessTokenGenerator)
    : IDoLoginUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        Validate(request);
        var user = await repository.GetActiveUserWithEmail(request.Email);
        if (user == null) throw new InvalidLoginException();

        var passwordIsValid = passwordEncripter.Verify(request.Password, user.Password);

        if (!passwordIsValid) throw new InvalidLoginException();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = accessTokenGenerator.Generate(user)
        };
    }

    private void Validate(RequestLoginJson request)
    {
        var validator = new DoLoginValidator();
        var result = validator.Validate(request);
        if (result.IsValid) return;

        var errorMessages =
            result
                .Errors
                .Select(f => f.ErrorMessage)
                .ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}