using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinCs.Domain.Entities;
using FinCs.Domain.Security.Tokens;
using FinCs.Domain.Services.LoggedUser;
using FinCs.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinCs.Infrastructure.Services.LoggedUser;

internal class LoggedUser : ILoggedUser
{
    private readonly FinCsDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(FinCsDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}