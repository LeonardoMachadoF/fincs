using FinCs.Communication.Requests;
using FinCs.Exception;
using FluentValidation;

namespace FinCs.Application.UseCases.Expenses.Update;

public class UpdateExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_0);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_EXPENSE_DATE);
        RuleFor(x => x.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}