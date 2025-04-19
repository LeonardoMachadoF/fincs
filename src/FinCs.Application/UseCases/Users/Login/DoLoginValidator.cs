using FinCs.Communication.Requests;
using FinCs.Exception;
using FluentValidation;

namespace FinCs.Application.UseCases.Users.Login;

public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestLoginJson>());
    }
}