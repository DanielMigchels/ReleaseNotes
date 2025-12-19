using PdfSharp.Fonts;

namespace ReleaseNotes.API.Services.Project;

public class CustomFontResolver : IFontResolver
{
    private readonly string arialPath = Path.Combine("Assets/Arial.ttf");
    private readonly string arialBoldPath = Path.Combine("Assets/Arial_bold.ttf");
    private readonly string arialItalic = Path.Combine("Assets/Arial_italic.ttf");

    public byte[] GetFont(string faceName)
    {
        if (faceName == "Arial#")
        {
            return File.ReadAllBytes(arialPath);
        }
        if (faceName == "Arial#b")
        {
            return File.ReadAllBytes(arialBoldPath);
        }
        if (faceName == "Arial#i")
        {
            return File.ReadAllBytes(arialItalic);
        }

        throw new InvalidOperationException("Font not found.");
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
        {
            if (isBold)
            {
                return new FontResolverInfo("Arial#b");
            }

            if (isItalic)
            {
                return new FontResolverInfo("Arial#i");
            }

            return new FontResolverInfo("Arial#");
        }

        throw new InvalidOperationException("Font not found.");
    }
}
