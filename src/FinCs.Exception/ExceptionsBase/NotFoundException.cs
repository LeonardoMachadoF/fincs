using System.Net;

namespace FinCs.Exception.ExceptionsBase;

public class NotFoundException(string message) : FinCsException(message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [message];
    }
}