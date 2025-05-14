using FinCs.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();
        mock.Setup(passwordEncripter =>
            passwordEncripter.Encript(It.IsAny<string>())
        ).Returns("!%@#dasdasW2#2");

        return mock.Object;
    }
}