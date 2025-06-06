using System.Net;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using Shouldly;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly string METHOD = "api/user";

    public RegisterUserTest(CustomWebApplicationFactory webAppFactory)
    {
        _httpClient = webAppFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}