using AutoMapper;
using FinCs.Communication.Responses;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase(
    ILoggedUser loggedUser,
    IExpensesReadOnlyRepository expensesRepository,
    IMapper mapper)
    : IGetExpenseByIdUseCase
{
    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var acLoggedUser = await loggedUser.Get();
        var expense = await expensesRepository.GetById(acLoggedUser, id);

        if (expense == null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return mapper.Map<ResponseExpenseJson>(expense);
    }
}