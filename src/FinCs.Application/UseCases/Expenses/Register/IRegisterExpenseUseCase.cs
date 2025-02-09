using FinCs.Communication.Requests;
using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request);
}