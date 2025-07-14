using System.Globalization;
using System.Net;
using System.Text.Json;
using FinCs.Communication.Enums;
using FinCs.Exception;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : FinCsClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly long _expenseId;
    private readonly string _token;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
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

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await DoGet($"{METHOD}/1000", _token, culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.Count().ShouldBe(1);
        errors.Any(error => error.GetString() == expectedMessage).ShouldBeTrue();
    }
}