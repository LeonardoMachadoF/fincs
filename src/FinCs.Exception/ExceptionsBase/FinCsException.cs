namespace FinCs.Exception.ExceptionsBase;

public abstract class FinCsException : SystemException
{
    protected FinCsException(string message) : base(message)
    {
    }

    public FinCsException()
    {
    }
}