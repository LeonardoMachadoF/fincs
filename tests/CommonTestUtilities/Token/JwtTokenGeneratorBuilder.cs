using FinCs.Domain.Entities;
using FinCs.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(
            generator => generator.Generate(
                It.IsAny<User>()
            )
        ).Returns("token");
        return mock.Object;
    }
}