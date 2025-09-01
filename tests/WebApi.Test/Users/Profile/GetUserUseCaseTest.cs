using System.Net;
using System.Text.Json;
using Shouldly;

namespace WebApi.Test.Users.Profile;

public class GetUserUseCaseTest : FinCsClassFixture
{
    private const string METHOD = "api/user";

    private readonly string _token;
    private readonly string _userEmail;
    private readonly string _userName;

    public GetUserUseCaseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _userName = webApplicationFactory.User_Team_Member.GetName();
        _userEmail = webApplicationFactory.User_Team_Member.GetEmail();
    }

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