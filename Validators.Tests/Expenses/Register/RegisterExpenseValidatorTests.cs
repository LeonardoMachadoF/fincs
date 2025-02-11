using CommonTestUtilities.Requests;
using FinCs.Application.UseCases.Expenses.Register;
using FinCs.Exception;
using Shouldly;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void ValidateRegisterExpense_ShouldReturnTrue_WhenRegisterExpenseIsValid()
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.BuildRequestRegisterExpenseJson();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("Title")]
    [InlineData("Date")]
    [InlineData("PaymentType")]
    [InlineData("Amount")]
    public void ValidateRegisterExpense_ShouldInvalidateGeneratedData(string invalidProperty)
    {
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.BuildInvalidRequestRegisterExpenseJson(invalidProperty);
        var errorMessages = new Dictionary<string, string>
        {
            { "Title", ResourceErrorMessages.TITLE_REQUIRED },
            { "Date", ResourceErrorMessages.INVALID_EXPENSE_DATE },
            { "PaymentType", ResourceErrorMessages.PAYMENT_TYPE_INVALID },
            { "Amount", ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_0 }
        };

        var result = validator.Validate(request);
        var expectedMessage = errorMessages[invalidProperty];

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(expectedMessage);
    }
}