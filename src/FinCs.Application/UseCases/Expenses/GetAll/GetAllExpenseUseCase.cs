using AutoMapper;
using FinCs.Communication.Responses;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;

namespace FinCs.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase(
    ILoggedUser loggedUser,
    IExpensesReadOnlyRepository expensesRepository,
    IMapper mapper)
    : IGetAllExpenseUseCase
{
    public async Task<ResponseExpensesJson> Execute()
    {
        var acLoggedUser = await loggedUser.Get();
        var result = await expensesRepository.GetAll(acLoggedUser);

        return new ResponseExpensesJson
        {
            Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}