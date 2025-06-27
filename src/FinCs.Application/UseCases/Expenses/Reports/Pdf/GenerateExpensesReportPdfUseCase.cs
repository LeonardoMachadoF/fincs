using FinCs.Application.UseCases.Expenses.Reports.Pdf.Colors;
using FinCs.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using FinCs.Domain.Extensions;
using FinCs.Domain.Reports;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using FontHelper = FinCs.Application.UseCases.Expenses.Reports.Pdf.Fonts.FontHelper;

namespace FinCs.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase
    : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE = 25;
    private readonly ILoggedUser _loggedUser;
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();
        var expenses = await _repository.GetByMonth(loggedUser, month);
        if (expenses.Count == 0) return [];

        var document = CreateDocument(loggedUser.Name, month);
        var page = CreatePage(document);

        CreateHeaderWithLogoAndName(loggedUser.Name, page);
        var totalSpent = expenses.Sum(expense => expense.Amount);
        CreateTotalSpentSection(page, month, totalSpent);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderToValue(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInfo(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInfo(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInfo(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) == false)
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPENSE;

                descriptionRow.Cells[0].AddParagraph(expense.Description);
                descriptionRow.Cells[0].Format.Font = new Font
                    { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }


            AddWhiteSpace(table);
        }

        return RenderPdf(document);
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount:f2} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private Document CreateDocument(string author, DateOnly month)
    {
        var document = new Document
        {
            Info =
            {
                Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}",
                Author = author
            }
        };

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.ROBOTO_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private byte[] RenderPdf(Document document)
    {
        var rendered = new PdfDocumentRenderer
        {
            Document = document
        };

        rendered.RenderDocument();
        using var file = new MemoryStream();
        rendered.PdfDocument.Save(file);

        return file.ToArray();
    }

    private void CreateHeaderWithLogoAndName(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        // var assembly = Assembly.GetExecutingAssembly();
        // var directoryName = Path.GetDirectoryName(assembly.Location);
        // var pathFile = Path.Combine(directoryName!, $"logo.png");
        // row.Cells[0].AddParagraph(pathFile);
        row.Cells[0].AddParagraph("*LOGO*");
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

        row.Cells[1].AddParagraph($"Hey, {name}");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalSpent)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 15 });
        paragraph.AddLineBreak();


        paragraph.AddFormattedText($"{totalSpent:f2} {CURRENCY_SYMBOL}",
            new Font { Name = FontHelper.ROBOTO_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderToValue(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInfo(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
}