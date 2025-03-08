using FinCs.Domain.Entities;

namespace FinCs.Domain.Security.Tokens;

public interface IAcessTokenGenerator
{
    string Generate(User user);
}