using FinCs.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace FinCs.Infrastructure.Security;

public class BCrypt : IPasswordEncripter
{
    public string Encript(string password)
    {
        var passwordHash = BC.HashPassword(password);
        return passwordHash;
    }
}