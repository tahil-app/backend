using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Reports;

public abstract class BaseReport
{
    public int MarginHorizontal { get; } = 20;
    public int MarginVertical { get; } = 20;

    public int HeaderFontSize { get; } = 11;
    public int HeaderImageSize { get; } = 40;
    public int FooterFontSize { get; } = 10;

    
    public byte[] GenerateReport(LocalizedStrings localized, string reportTitle, string reportName, Action<IContainer> content) 
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.MarginHorizontal(MarginHorizontal);
                page.MarginVertical(MarginVertical);

                page.Header().Element(headerContainer =>
                    GenerateHeader(headerContainer, reportTitle, reportName));

                page.Content().Element(content);

                page.Footer().Element(footerContainer =>
                    GenerateFooter(footerContainer, localized));
            });
        });

        return document.GeneratePdf();
    }

    public virtual void GenerateHeader(IContainer container, string reportName, string name = "", string clientLogo = "")
    {
        container.Column(column =>
        {
            // Header content
            column.Item().Row(row =>
            {
                // Left side - Logo
                row.RelativeItem(1).Width(HeaderImageSize).Column(logoColumn =>
                {
                    logoColumn.Item().Row(logoRow =>
                    {
                        logoRow.AutoItem().Width(HeaderImageSize).Height(HeaderImageSize).Image("wwwroot/Images/logo.png").FitArea();
                    });
                });

                // Center side - Report title
                row.RelativeItem(10).PaddingTop(15).Column(titleColumn =>
                {
                    titleColumn.Item().AlignCenter().Text(text =>
                    {
                        text.Span(reportName)
                            .FontSize(HeaderFontSize)
                            .FontColor(Colors.Grey.Darken2)
                            .Bold();

                        if (!string.IsNullOrEmpty(name))
                        {
                            text.Span($" - ")
                                .FontSize(HeaderFontSize)
                                .FontColor(Colors.Grey.Darken2)
                                .Bold();

                            text.Span(name)
                                .FontSize(HeaderFontSize)
                                .FontColor(Colors.Grey.Darken3)
                                .Bold();
                        }
                    });
                });

                // Right side - Client logo
                row.RelativeItem(1).Width(HeaderImageSize).Column(logoColumn =>
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        logoColumn.Item().Row(logoRow =>
                        {
                            logoRow.AutoItem().Width(HeaderImageSize).Height(HeaderImageSize).Image("wwwroot/Images/logo.png").FitArea();
                        });
                    }
                });
            });

            // Separator line
            column.Item().PaddingTop(3).PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
        });
    }

    public virtual void GenerateFooter(IContainer container, LocalizedStrings localized)
    {
        container.Column(column =>
        {
            // Line at the top
            column.Item().PaddingTop(10).PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            // Footer content with date on left and page numbering on right
            column.Item().Row(row =>
            {
                var footerColor = Colors.Grey.Darken1;
                var currentDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Left side - Date in Arabic, Page numbers in English
                row.RelativeItem().AlignLeft().Text(text =>
                {
                    if (localized.IsAr)
                    {
                        text.CurrentPageNumber().FontSize(FooterFontSize).FontColor(footerColor);
                        text.Span(" - ").FontSize(FooterFontSize).FontColor(footerColor);
                        text.TotalPages().FontSize(FooterFontSize).FontColor(footerColor);
                    }
                    else
                    {
                        text.Span(currentDate)
                            .FontSize(FooterFontSize)
                            .FontColor(footerColor);
                    }
                });

                // Center - Always App Name
                row.RelativeItem().AlignCenter().Text(localized.AppName)
                    .FontSize(FooterFontSize)
                    .FontColor(footerColor);

                // Right side - Page numbers in Arabic, Date in English
                row.RelativeItem().AlignRight().Text(text =>
                {
                    if (localized.IsAr)
                    {
                        text.Span(currentDate)
                            .FontSize(FooterFontSize)
                            .FontColor(footerColor);
                    }
                    else
                    {
                        text.CurrentPageNumber().FontSize(FooterFontSize).FontColor(footerColor);
                        text.Span(" - ").FontSize(FooterFontSize).FontColor(footerColor);
                        text.TotalPages().FontSize(FooterFontSize).FontColor(footerColor);
                    }
                });
            });
        });
    }
}