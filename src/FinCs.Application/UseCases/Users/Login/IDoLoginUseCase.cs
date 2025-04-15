using FinCs.Communication.Requests;
using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Users.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}