using System.Reflection;
using PdfSharp.Fonts;

namespace FinCs.Application.UseCases.Expenses.Reports.Pdf.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);
        stream ??= ReadFontFile(FontHelper.DEFAULT_FONT);

        var length = stream!.Length;

        var data = new byte[length];
        stream.ReadExactly(data, 0, (int)length);

        return data;
    }

    private Stream? ReadFontFile(string familyName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(
            $"FinCs.Application.UseCases.Expenses.Reports.Pdf.Fonts.{familyName}.ttf");
    }
}