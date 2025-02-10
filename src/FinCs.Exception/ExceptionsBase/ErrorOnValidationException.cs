namespace FinCs.Exception.ExceptionsBase;

public class ErrorOnValidationException : FinCsException
{
    public ErrorOnValidationException(List<string> errorMessages)
    {
        Errors = errorMessages;
    }

    public List<string> Errors { get; set; }
}