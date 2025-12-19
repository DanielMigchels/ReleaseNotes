using ReleaseNotes.API.Enums;
using ReleaseNotes.API.Services.Bundle.Models;
using ReleaseNotes.API.Services.Project;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace ReleaseNotes.API.Services.Bundle;

public class BundlePdfGeneratorService : IBundlePdfGeneratorService
{
    private const double Margin = 50;
    private const double BottomMargin = 80;
    private const double LineHeight = 15;

    public Stream GeneratePdf(BundleReleaseTimeRangesResponseModel bundle)
    {
        var document = new PdfDocument();
        var pageNumber = 0;
        var y = Margin;

        var page = AddPage(document, ref pageNumber, ref y);
        var gfx = XGraphics.FromPdfPage(page);

        GlobalFontSettings.FontResolver = new CustomFontResolver();

        var heading0Font = new XFont("Arial", 18, XFontStyleEx.Bold);
        var heading1Font = new XFont("Arial", 14, XFontStyleEx.Bold);
        var heading2Font = new XFont("Arial", 12, XFontStyleEx.Bold);
        var textFont = new XFont("Arial", 10);
        var additionFont = new XFont("Arial", 8, XFontStyleEx.Italic);

        DrawPageNumber(gfx, pageNumber, page);

        y = DrawHeader(gfx, bundle.BundleName + " - " + bundle.Version, heading0Font, y);

        DrawLegend(gfx, textFont, additionFont);

        foreach (var releaseNote in bundle.ReleaseNote)
        {
            var noteEntries = releaseNote.Releases.SelectMany(x => x.NoteEntries).Count();
            if (noteEntries == 0)
            {
                continue;
            }

            y = DrawTitle(gfx, releaseNote.Name, heading1Font, y);

            foreach (var release in releaseNote.Releases)
            {
                if (release.NoteEntries.Count() == 0)
                {
                    continue;
                }

                gfx = EnsureSpaceForContent(document, ref page, ref gfx, ref pageNumber, ref y);

                gfx.DrawString(release.Version, heading2Font, XBrushes.Black, new XRect(Margin, y, page.Width.Point - BottomMargin, page.Height.Point - BottomMargin), XStringFormats.TopLeft);
                gfx.DrawString(release.CreatedOnUtc.ToString("dd-MM-yyyy"), additionFont, XBrushes.Gray, new XRect(Margin, y, page.Width.Point - BottomMargin, page.Height.Point - BottomMargin), XStringFormats.TopRight);
                y += LineHeight + 5;

                foreach (var entry in release.NoteEntries)
                {
                    gfx = EnsureSpaceForContent(document, ref page, ref gfx, ref pageNumber, ref y);

                    string entryType = GetEntryTypeSymbol(entry.Type ?? default);
                    double entryTypeX = Margin + 20 - (gfx.MeasureString(entryType, textFont).Width / 2);
                    gfx.DrawString(entryType, textFont, XBrushes.Black, new XRect(entryTypeX, y, page.Width.Point - BottomMargin, page.Height.Point - BottomMargin), XStringFormats.TopLeft);

                    DrawWrappedText(gfx, entry.Text, textFont, Margin + 30, ref y, 455, LineHeight);
                }

                y += LineHeight;
            }
        }

        var stream = new MemoryStream();
        document.Save(stream);
        stream.Position = 0;
        return stream;
    }


    private static void DrawWrappedText(XGraphics gfx, string text, XFont textFont, double x, ref double y, double maxWidth, double lineHeight, XStringFormat? format = null, XBrush? brush = null)
    {
        format ??= XStringFormats.TopLeft;
        brush ??= XBrushes.Black;
        var words = text.Split(' ');
        var line = "";
        var lines = new List<string>();

        foreach (var word in words)
        {
            var testLine = line.Length > 0 ? $"{line} {word}" : word;
            var testWidth = gfx.MeasureString(testLine, textFont).Width;

            if (testWidth <= maxWidth)
            {
                line = testLine;
            }
            else
            {
                lines.Add(line);
                line = word;
            }
        }
        if (line.Length > 0)
        {
            lines.Add(line);
        }

        foreach (var textLine in lines)
        {
            gfx.DrawString(textLine, textFont, brush, new XRect(x, y, maxWidth, lineHeight), format);
            y += lineHeight;
        }
    }

    private static string GetEntryTypeSymbol(NoteEntryType type)
    {
        return type switch
        {
            NoteEntryType.Critical => "!",
            NoteEntryType.NewFeature => "+",
            NoteEntryType.Bugfix => "•",
            NoteEntryType.Removal => "-",
            _ => "•"
        };
    }

    private static PdfPage AddPage(PdfDocument document, ref int pageNumber, ref double y)
    {
        var page = document.AddPage();
        pageNumber++;
        y = Margin;

        return page;
    }

    private static XGraphics EnsureSpaceForContent(PdfDocument document, ref PdfPage page, ref XGraphics gfx, ref int pageNumber, ref double y)
    {
        if (y > page.Height.Point - BottomMargin)
        {
            page = AddPage(document, ref pageNumber, ref y);
            gfx = XGraphics.FromPdfPage(page);
            DrawPageNumber(gfx, pageNumber, page);
        }
        return gfx;
    }

    private static void DrawPageNumber(XGraphics gfx, int pageNumber, PdfPage page)
    {
        var additionFont = new XFont("Arial", 10, XFontStyleEx.Italic);
        gfx.DrawString($"{pageNumber}", additionFont, XBrushes.Gray, new XRect(Margin, Margin, page.Width.Point - BottomMargin, page.Height.Point - BottomMargin), XStringFormats.BottomRight);
    }

    private static double DrawHeader(XGraphics gfx, string releaseNotesName, XFont heading1Font, double y)
    {
        var logoImage = XImage.FromFile("Assets/logo.png");
        var scaleFactor = 0.25;
        gfx.DrawImage(logoImage, Margin, y, 570 * scaleFactor, 204 * scaleFactor);
        y += 204 * scaleFactor + 20;

        gfx.DrawString(releaseNotesName, heading1Font, XBrushes.Black, new XRect(Margin, y, gfx.PdfPage!.Width.Point - BottomMargin, gfx.PdfPage.Height.Point - BottomMargin), XStringFormats.TopLeft);
        y += 40;

        return y;
    }

    private static double DrawTitle(XGraphics gfx, string releaseNotesName, XFont heading1Font, double y)
    {
        gfx.DrawString(releaseNotesName, heading1Font, XBrushes.Black, new XRect(Margin, y, gfx.PdfPage!.Width.Point - BottomMargin, gfx.PdfPage.Height.Point - BottomMargin), XStringFormats.TopLeft);
        y += 40;

        return y;
    }

    private static void DrawLegend(XGraphics gfx, XFont textFont, XFont additionFont)
    {
        var legendX = gfx.PdfPage!.Width.Point - Margin - 125 + 11;
        var legendY = Margin;
        var legendLineHeight = 10;

        var entryType = "!";
        var entryTypeX = legendX - (gfx.MeasureString(entryType, textFont).Width / 2);
        gfx.DrawString(entryType, additionFont, XBrushes.Gray, new XRect(entryTypeX, legendY, 20, 20), XStringFormats.TopLeft);
        gfx.DrawString("Critical issue", additionFont, XBrushes.Gray, new XRect(legendX + 25, legendY, 75, 20), XStringFormats.TopLeft);
        legendY += legendLineHeight;

        entryType = "+";
        entryTypeX = legendX - (gfx.MeasureString(entryType, textFont).Width / 2);
        gfx.DrawString(entryType, additionFont, XBrushes.Gray, new XRect(entryTypeX, legendY, 20, 20), XStringFormats.TopLeft);
        gfx.DrawString("New functionality", additionFont, XBrushes.Gray, new XRect(legendX + 25, legendY, 75, 20), XStringFormats.TopLeft);
        legendY += legendLineHeight;

        entryType = "•";
        entryTypeX = legendX - (gfx.MeasureString(entryType, textFont).Width / 2);
        gfx.DrawString(entryType, additionFont, XBrushes.Gray, new XRect(entryTypeX, legendY, 20, 20), XStringFormats.TopLeft);
        gfx.DrawString("Adjustment", additionFont, XBrushes.Gray, new XRect(legendX + 25, legendY, 75, 20), XStringFormats.TopLeft);
        legendY += legendLineHeight;

        entryType = "-";
        entryTypeX = legendX - (gfx.MeasureString(entryType, textFont).Width / 2);
        gfx.DrawString(entryType, additionFont, XBrushes.Gray, new XRect(entryTypeX, legendY, 20, 20), XStringFormats.TopLeft);
        gfx.DrawString("Functionality removed", additionFont, XBrushes.Gray, new XRect(legendX + 25, legendY, 75, 20), XStringFormats.TopLeft);
    }
}
