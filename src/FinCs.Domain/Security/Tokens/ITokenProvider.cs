namespace FinCs.Domain.Security.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}