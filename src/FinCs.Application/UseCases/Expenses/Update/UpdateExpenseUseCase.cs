using AutoMapper;
using FinCs.Communication.Requests;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase(IMapper mapper, IExpensesUpdateOnlyRepository repository, IUnitOfWork unitOfWork)
    : IUpdateExpenseUseCase
{
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        var expense = await repository.GetById(id);
        if (expense is null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        mapper.Map(request, expense);

        repository.Update(expense);
        await unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new UpdateExpenseValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList()
            );
    }
}