using System.Net;
using System.Text.Json;
using Shouldly;

namespace WebApi.Test.Users.Profile;

public class GetUserUseCaseTest(CustomWebApplicationFactory webApplicationFactory)
    : FinCsClassFixture(webApplicationFactory)
{
    private const string METHOD = "api/user";

    private readonly string _token = webApplicationFactory.User_Team_Member.GetToken();
    private readonly string _userEmail = webApplicationFactory.User_Team_Member.GetEmail();
    private readonly string _userName = webApplicationFactory.User_Team_Member.GetName();

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_userName);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail);
    }
}