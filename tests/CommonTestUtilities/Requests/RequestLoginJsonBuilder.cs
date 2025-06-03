using Bogus;
using FinCs.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker =>
                "Aa1!" + faker.Random.String2(5));
    }
}