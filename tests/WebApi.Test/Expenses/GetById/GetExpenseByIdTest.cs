using System.Net;
using System.Text.Json;
using FinCs.Communication.Enums;
using Shouldly;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : FinCsClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly long _expenseId;
    private readonly string _token;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
        _expenseId = webApplicationFactory.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet($"{METHOD}/{_expenseId}", _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
        response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThanOrEqualTo(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue();
    }
}