using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Entities;
using FinCs.Domain.Enums;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(IExpensesRepository expensesRepository, IUnitOfWork unitOfWork)
    : IRegisterExpenseUseCase
{
    public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);
        var entity = new Expense
        {
            Title = request.Title,
            Description = request.Description,
            Amount = request.Amount,
            Date = request.Date,
            PaymentType = (PaymentType)request.PaymentType
        };

        await expensesRepository.Add(entity);

        await unitOfWork.Commit();

        return new ResponseRegisterExpenseJson
        {
            Title = request.Title
        };
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