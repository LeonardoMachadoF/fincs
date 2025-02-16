using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Expenses.GetAll;

public interface IGetAllExpenseUseCase
{
    Task<ResponseExpensesJson> Execute();
}