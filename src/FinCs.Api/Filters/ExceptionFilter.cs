using FinCs.Communication.Responses;
using FinCs.Exception;
using FinCs.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinCs.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FinCsException)
            HandleProjectException(context);
        else
            ThrowUnknownException(context);
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var finCsException = (FinCsException)context.Exception;
        var errorResponse = new ResponseErrorJson(finCsException.GetErrors());
        context.HttpContext.Response.StatusCode = finCsException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnknownException(ExceptionContext context)
    {
        Console.WriteLine(context.Exception);
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}