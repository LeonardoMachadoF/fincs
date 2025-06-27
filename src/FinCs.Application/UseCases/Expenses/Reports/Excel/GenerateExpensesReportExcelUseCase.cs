using ClosedXML.Excel;
using FinCs.Application.UseCases.Report.Excel;
using FinCs.Domain.Extensions;
using FinCs.Domain.Reports;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;

namespace FinCs.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";

    public async Task<byte[]> Execute(DateOnly month)
    {
        var acLoggedUser = await loggedUser.Get();
        var expenses = await repository.GetByMonth(acLoggedUser, month);
        if (expenses.Count == 0) return [];

        using var workBook = new XLWorkbook();
        workBook.Author = acLoggedUser.Name;
        workBook.Style.Font.FontSize = 12;
        workBook.Style.Font.FontName = "Times New Roman";

        var worksheet = workBook.Worksheets.Add(month.ToString("Y"));
        InsertHeader(worksheet);

        var row = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{row}").Value = expense.Title;
            worksheet.Cell($"B{row}").Value = expense.Date;
            worksheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cell($"D{row}").Value = expense.Amount;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL}#,##0.00";


            worksheet.Cell($"E{row}").Value = expense.Description;

            row++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workBook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;
        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#82E0AA");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}