using System.Net;
using System.Text.Json;
using Shouldly;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest : FinCsClassFixture
{
    private readonly string _token;

    private readonly string METHOD = "api/expenses";

    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        response.RootElement.GetProperty("expenses").EnumerateArray().ShouldNotBeEmpty();
    }
}