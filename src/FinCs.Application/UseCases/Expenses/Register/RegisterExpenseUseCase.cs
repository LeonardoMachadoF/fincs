using AutoMapper;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Entities;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(
    IExpensesWriteOnlyRepository expensesRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRegisterExpenseUseCase
{
    public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);
        var entity = mapper.Map<Expense>(request);

        await expensesRepository.Add(entity);

        await unitOfWork.Commit();

        return mapper.Map<ResponseRegisterExpenseJson>(entity);
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        var validator = new RegisterExpenseValidator();
        var result = validator.Validate(request);


        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(
                error => error.ErrorMessage
            ).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}