using Microsoft.AspNetCore.Http;

namespace FinCs.Infrastructure.Security.Tokens;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }

    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers["Authorization"].ToString();


        return authorization["Bearer ".Length..].Trim();
    }
}