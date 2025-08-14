using System.Net.Mime;
using FinCs.Application.UseCases.Expenses.Reports.Pdf;
using FinCs.Application.UseCases.Report.Excel;
using FinCs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinCs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IGenerateExpensesReportExcelUseCase useCase,
        [FromQuery] DateOnly month)
    {
        var file = await useCase.Execute(month);
        if (file.Length == 0)
            return NoContent();
        return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IGenerateExpensesReportPdfUseCase useCase,
        [FromQuery] DateOnly month)
    {
        var file = await useCase.Execute(month);
        if (file.Length == 0)
            return NoContent();
        return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
    }
}