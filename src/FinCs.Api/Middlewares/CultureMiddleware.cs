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
        var acceptLang = context.Request.Headers["Accept-Language"]
            .ToString()
            .Split(',')
            .Select(l => l.Split(';').First().Trim())
            .FirstOrDefault();

        var cultureInfo = acceptLang != null &&
                          _supportedLanguages.Contains(acceptLang, StringComparer.OrdinalIgnoreCase)
            ? new CultureInfo(acceptLang)
            : new CultureInfo("en");

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}