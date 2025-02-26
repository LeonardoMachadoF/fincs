using AutoMapper;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Entities;
using FinCs.Domain.Security.Cryptography;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        Validate(request);
        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encript(request.Password);

        return new ResponseRegisterUserJson
        {
            Name = user.Name
        };
    }

    private void Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}