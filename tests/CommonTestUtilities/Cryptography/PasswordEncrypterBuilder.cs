using FinCs.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();

        _mock.Setup(passwordEncripter =>
            passwordEncripter.Encript(It.IsAny<string>())
        ).Returns("!%@#dasdasW2#2");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) == false)
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);

        return this;
    }


    public IPasswordEncripter Build()
    {
        return _mock.Object;
    }
}