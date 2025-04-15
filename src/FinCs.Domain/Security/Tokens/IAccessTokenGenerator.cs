using FinCs.Domain.Entities;

namespace FinCs.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}