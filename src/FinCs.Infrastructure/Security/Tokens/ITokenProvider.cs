namespace FinCs.Infrastructure.Security.Tokens;

public interface ITokenProvider
{
    string TokenOnRequest();
}