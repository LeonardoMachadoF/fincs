using FinCs.Communication.Requests;
using FluentValidation;

namespace FinCs.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
        RuleFor(x => x.PaymentType).IsInEnum().WithMessage("Payment Type is invalid.");
    }
}