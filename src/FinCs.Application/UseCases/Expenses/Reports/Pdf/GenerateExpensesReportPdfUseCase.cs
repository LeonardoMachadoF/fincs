using FinCs.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using FinCs.Domain.Repositories.Expenses;
using PdfSharp.Fonts;

namespace FinCs.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository _repository)
    : IGenerateExpensesReportPdfUseCase
{
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.GetByMonth(month);
        if (expenses.Count == 0) return [];
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        return [];
    }
}