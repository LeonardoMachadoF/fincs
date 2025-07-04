using AutoMapper;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Domain.Entities;
using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(
    IExpensesWriteOnlyRepository expensesRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggedUser loggedUser
)
    : IRegisterExpenseUseCase
{
    public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);

        var acLoggedUser = await loggedUser.Get();
        var expense = mapper.Map<Expense>(request);
        expense.UserId = acLoggedUser.Id;

        await expensesRepository.Add(expense);

        await unitOfWork.Commit();

        return mapper.Map<ResponseRegisterExpenseJson>(expense);
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