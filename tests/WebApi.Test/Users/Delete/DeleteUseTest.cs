using System.Net;
using Shouldly;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTest(CustomWebApplicationFactory webApplicationFactory)
    : FinCsClassFixture(webApplicationFactory)
{
    private const string METHOD = "api/user";
    private readonly string _token = webApplicationFactory.User_Team_Member.GetToken();

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(METHOD, _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}