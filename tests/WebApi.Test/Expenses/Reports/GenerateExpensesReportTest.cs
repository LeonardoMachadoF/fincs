using System.Net;
using System.Net.Mime;
using Shouldly;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpensesReportTest : FinCsClassFixture
{
    private const string METHOD = "api/Report";

    private readonly string _adminToken;
    private readonly DateTime _expenseDate;
    private readonly string _teamMemberToken;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet($"{METHOD}/pdf?month={_expenseDate:Y}", _adminToken);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet($"{METHOD}/excel?month={_expenseDate:Y}", _adminToken);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet($"{METHOD}/excel?month={_expenseDate:Y}", _teamMemberToken);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet($"{METHOD}/pdf?month={_expenseDate:Y}", _teamMemberToken);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}