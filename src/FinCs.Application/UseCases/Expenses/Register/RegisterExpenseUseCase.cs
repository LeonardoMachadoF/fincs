using FinCs.Communication.Enums;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase:IRegisterExpenseUseCase
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
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            errors.Add("Title is required.");
        }

        if (request.Amount <= 0)
        {
            errors.Add("Amount is required and must be greater than 0.");
        }

        if (DateTime.Compare(request.Date, DateTime.UtcNow) > 0)
        {
            errors.Add("Date cannot be greater than the current date.");
        }

        if (!Enum.IsDefined(typeof(PaymentType), request.PaymentType))
        {
            errors.Add("Payment type is invalid.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join("|", errors));
        }
    }

}

