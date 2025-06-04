using CommonTestUtilities.Requests;
using FinCs.Application.UseCases.Users.Login;
using FinCs.Exception;
using Shouldly;

namespace Validators.Tests.Users.Login;

public class LoginUserValidatorTest
{
    private readonly DoLoginValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestLoginJsonBuilder.Build();
        var result = _validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Should_Return_EmailEmpty_When_Email_Is_Null_Or_Whitespace(string email)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("invalid@")]
    [InlineData("@domain.com")]
    public void Should_Return_EmailInvalid_When_Email_Format_Is_Wrong(string email)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID);
    }
}