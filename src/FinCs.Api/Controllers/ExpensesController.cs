using FinCs.Application.UseCases.Expenses.Register;
using FinCs.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FinCs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RequestRegisterExpenseJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}