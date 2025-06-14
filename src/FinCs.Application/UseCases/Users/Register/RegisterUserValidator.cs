using System.Globalization;
using FinCs.Communication.Requests;
using FinCs.Exception;
using FluentValidation;

namespace FinCs.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(_ =>
                ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", CultureInfo.CurrentUICulture));

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(_ =>
                ResourceErrorMessages.ResourceManager.GetString("EMAIL_EMPTY", CultureInfo.CurrentUICulture))
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(_ =>
                ResourceErrorMessages.ResourceManager.GetString("EMAIL_INVALID", CultureInfo.CurrentUICulture));

        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}