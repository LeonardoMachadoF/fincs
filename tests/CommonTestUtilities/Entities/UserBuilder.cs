using Bogus;
using CommonTestUtilities.Cryptography;
using FinCs.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncrypter.Encript(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid());

        return user;
    }
}