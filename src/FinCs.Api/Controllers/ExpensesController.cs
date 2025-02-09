using FinCs.Application.UseCases.Expenses.Register;
using FinCs.Communication.Requests;
using FinCs.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinCs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromServices] IRegisterExpenseUseCase useCase,
        [FromBody] RequestRegisterExpenseJson request)
    {
        try
        {
            var response = useCase.Execute(request);
            return Created(string.Empty, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new ResponseErrorJson(ex.Message);
            return BadRequest(errorResponse);
        }
        catch
        {
            var errorResponse = new ResponseErrorJson("Internal Server Error");
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}