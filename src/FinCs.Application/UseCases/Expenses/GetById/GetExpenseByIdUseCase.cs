using AutoMapper;
using FinCs.Communication.Responses;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
    : IGetExpenseByIdUseCase
{
    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var expense = await expensesRepository.GetById(id);

        if (expense == null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return mapper.Map<ResponseExpenseJson>(expense);
    }
}