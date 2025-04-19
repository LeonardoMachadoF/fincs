using CommonTestUtilities.Requests;
using FinCs.Application.UseCases.Users.Register;
using FinCs.Exception;
using Shouldly;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Name_Invalid(string name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.First().ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.First().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.First().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.First().ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD);
    }
}