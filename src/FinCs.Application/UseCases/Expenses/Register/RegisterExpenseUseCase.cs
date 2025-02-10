using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using FinCs.Exception.ExceptionsBase;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        Validate(request);
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