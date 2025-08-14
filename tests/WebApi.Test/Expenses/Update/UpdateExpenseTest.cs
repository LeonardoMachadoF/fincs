using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinCs.Exception;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTest : FinCsClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly long _expenseId;
    private readonly string _token;

    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut($"{METHOD}/{_expenseId}", request, _token);
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut($"{METHOD}/{_expenseId}", request, _token, culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray().ToList();

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        errors.Count.ShouldBe(1);
        errors.Any(error => error.GetString()!.Equals(expectedMessage)).ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut($"{METHOD}/1000", request, _token, culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray().ToList();

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.Count.ShouldBe(1);
        errors.Any(error => error.GetString()!.Equals(expectedMessage)).ShouldBeTrue();
    }
}