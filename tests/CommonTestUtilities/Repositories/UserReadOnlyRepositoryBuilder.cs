using FinCs.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    public static IUserReadOnlyRepository Build()
    {
        var mock = new Mock<IUserReadOnlyRepository>();
        return mock.Object;
    }
}