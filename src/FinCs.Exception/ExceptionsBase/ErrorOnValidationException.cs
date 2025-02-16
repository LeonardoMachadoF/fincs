using System.Net;

namespace FinCs.Exception.ExceptionsBase;

public class ErrorOnValidationException(List<string> errorMessages) : FinCsException
{
    private List<string> Errors { get; } = errorMessages;
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return Errors;
    }
}