using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinCs.Domain.Entities;
using FinCs.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace FinCs.Infrastructure.Security.Tokens;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes;
    private readonly string _signingKey;

    public JwtTokenGenerator(uint expirationTimeInMinutes, string signingKey)
    {
        _expirationTimeInMinutes = expirationTimeInMinutes;
        _signingKey = signingKey;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            new(ClaimTypes.Role, user.Role)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials =
                new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }
}