using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinCs.Communication.Requests;
using FinCs.Exception;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string _email;
    private readonly HttpClient _httpClient;
    private readonly string _name;
    private readonly string _password;
    private readonly string METHOD = "api/login";

    public DoLoginTest(CustomWebApplicationFactory webAppFactory)
    {
        _httpClient = webAppFactory.CreateClient();
        _email = webAppFactory.GetEmail();
        _name = webAppFactory.GetName();
        _password = webAppFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(METHOD, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
            new StringWithQualityHeaderValue(culture));

        var response = await _httpClient.PostAsJsonAsync(METHOD, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement
            .GetProperty("errors")
            .EnumerateArray()
            .ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager
            .GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Count.ShouldBe(1);
        errors.Any(c => c.GetString()!.Equals(expectedMessage)).ShouldBeTrue();
    }
}