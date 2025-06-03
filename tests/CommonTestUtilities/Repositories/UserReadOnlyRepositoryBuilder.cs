using FinCs.Domain.Entities;
using FinCs.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistsActiveUserWithEmail(string email)
    {
        _repository.Setup(x => x.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetActiveUserWithEmail(user.Email)).ReturnsAsync(user);
        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}