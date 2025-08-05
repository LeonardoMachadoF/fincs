using System.Globalization;
using System.Net;
using System.Text.Json;
using FinCs.Exception;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseTest : FinCsClassFixture
{
    private readonly long _expenseId;
    private readonly string _token;
    private readonly string METHOD = "api/expenses";

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(
            $"{METHOD}/{_expenseId}",
            _token
        );

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(
            $"{METHOD}/{_expenseId}",
            _token
        );

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }


    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await DoDelete(
            $"{METHOD}/1000",
            _token,
            culture
        );

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement
            .GetProperty("errors")
            .EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager
            .GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldNotBeEmpty();
        errors.ShouldHaveSingleItem();

        var errorStrings = errors.Select(e => e.GetString()).ToList();

        errorStrings.ShouldContain(expectedMessage);
    }
}