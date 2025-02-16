using AutoMapper;
using FinCs.Communication.Responses;
using FinCs.Domain.Repositories.Expenses;

namespace FinCs.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase(IExpensesRepository expensesRepository, IMapper mapper) : IGetAllExpenseUseCase
{
    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await expensesRepository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}