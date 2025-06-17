using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IExpensesReadOnlyRepository expensesReadOnlyRepository,
    IExpensesWriteOnlyRepository expensesWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser) : IDeleteExpenseUseCase
{
    public async Task Execute(long id)
    {
        var acLoggedUser = await loggedUser.Get();
        var expense = await expensesReadOnlyRepository.GetById(acLoggedUser, id);
        if (expense is null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await expensesWriteOnlyRepository.Delete(id);

        await unitOfWork.Commit();
    }
}