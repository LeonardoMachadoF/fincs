using FinCs.Communication.Requests;
using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}