using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}