using System.Globalization;

namespace FinCs.Api.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _supportedLanguages;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
        _supportedLanguages = new List<string> { "en", "pt-PT", "pt-BR" };
    }

    public async Task Invoke(HttpContext context)
    {
        var requestCulture = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault()?.Trim();

        var cultureInfo = requestCulture != null && _supportedLanguages.Contains(requestCulture)
            ? new CultureInfo(requestCulture)
            : new CultureInfo("en");

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}