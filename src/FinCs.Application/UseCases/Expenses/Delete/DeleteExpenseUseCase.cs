using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IExpensesWriteOnlyRepository expensesWriteOnlyRepository,
    IUnitOfWork unitOfWork) : IDeleteExpenseUseCase
{
    public async Task Execute(long id)
    {
        var result = await expensesWriteOnlyRepository.Delete(id);
        if (result is false) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await unitOfWork.Commit();
    }
}