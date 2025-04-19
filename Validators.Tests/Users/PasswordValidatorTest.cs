using FinCs.Application.UseCases.Users;
using FinCs.Communication.Requests;
using FluentValidation;
using Shouldly;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("aaaaaaaA")]
    [InlineData("aaaaaaA1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();


        var result = validator
            .IsValid(
                new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()),
                password
            );

        result.ShouldBeFalse();
    }
}