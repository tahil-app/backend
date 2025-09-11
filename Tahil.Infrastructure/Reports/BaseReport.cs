using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Reports;

public abstract class BaseReport
{
    public int MarginHorizontal { get; } = 20;
    public int MarginVertical { get; } = 20;
    public int BorderRadius { get; set; } = 3;
    public string BorderColor { get; set; } = "#dfdfdf";

    public int HeaderFontSize { get; } = 11;
    public int HeaderImageSize { get; } = 40;
    public int FooterFontSize { get; } = 10;

    protected LocalizedStrings Localized { get; }
    protected BaseReport(LocalizedStrings localized)
    {
        Localized = localized;
    }


    public byte[] GenerateReport(string reportTitle, string reportName, Action<IContainer> content)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.MarginHorizontal(MarginHorizontal);
                page.MarginVertical(MarginVertical);

                page.Header().Element(headerContainer => GenerateHeader(headerContainer, reportTitle, reportName));

                page.Content().Element(content);

                page.Footer().Element(footerContainer => GenerateFooter(footerContainer));
            });
        });

        return document.GeneratePdf();
    }

    protected virtual void GenerateHeader(IContainer container, string reportName, string name = "", string clientLogo = "")
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
                    if (!string.IsNullOrEmpty(clientLogo))
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

    protected virtual void GenerateFooter(IContainer container)
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
                    if (Localized.IsAr)
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
                row.RelativeItem().AlignCenter().Text(Localized.AppName)
                    .FontSize(FooterFontSize)
                    .FontColor(footerColor);

                // Right side - Page numbers in Arabic, Date in English
                row.RelativeItem().AlignRight().Text(text =>
                {
                    if (Localized.IsAr)
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

    protected virtual void GenerateTextHeader(IContainer container, WeekDays? day = null , string? text = null)
    {
        text = text ?? Localized.GetDayName(day!.Value);

        var textContainer = container.Background(Color.FromHex("#f8f9fa"))
            .BorderRight(Localized.IsAr ? 0 : 3).BorderLeft(Localized.IsAr ? 3 : 0).BorderColor(Color.FromHex("#559c8b"))
            .CornerRadius(BorderRadius)
            .Padding(8)
            .Text(text)
            .FontSize(11)
            .Bold()
            .FontColor(Colors.Grey.Darken3);

        if (Localized.IsAr) textContainer.AlignRight();
        else textContainer.AlignLeft();
    }

    protected virtual void GenerateTableHeaderCell(IContainer container, string title, bool alignCenter = false)
    {
        container.Background(Color.FromHex("#f5f5f5")).Element(container =>
        {
            var textContainer = container.Border(1)
                .BorderColor(Color.FromHex(BorderColor))
                .Padding(5)
                .Text(title)
                .FontSize(10)
                .FontColor(Colors.Grey.Darken2)
                .Bold();

            if (Localized.IsAr) textContainer.AlignRight();
            else textContainer.AlignLeft();

            if (alignCenter)
                textContainer.AlignCenter();
        });
    }

    protected virtual void GenerateTableBodyCell(IContainer container, string title, bool alignCenter = false)
    {
        container.Element(container =>
        {
            var textContainer = container.Border(1)
                .Background(Colors.White)
                .BorderColor(Color.FromHex(BorderColor))
                .Padding(5)
                .Text(title)
                .FontSize(9)
                .FontColor(Colors.Grey.Darken3);

            if (Localized.IsAr) textContainer.AlignRight();
            else textContainer.AlignLeft();

            if(alignCenter)
                textContainer.AlignCenter();
        });
    }

    protected virtual void GenerateAttendanceStatusCell(IContainer container, bool isPresent, bool isLate, bool isAbsent)
    {
        container.Element(container =>
        {
            var statusText = "";
            var statusColor = Colors.Grey.Darken2;
            
            if (isPresent)
            {
                statusText = "✅";
                statusColor = Color.FromHex("#28a745"); // Green
            }
            else if (isLate)
            {
                statusText = "⚠️";
                statusColor = Color.FromHex("#fd7e14"); // Orange
            }
            else if (isAbsent)
            {
                statusText = "❌";
                statusColor = Color.FromHex("#dc3545"); // Red
            }
            else
            {
                statusText = "☐";
                statusColor = Colors.Grey.Darken2;
            }

            var textContainer = container.Border(1)
                .Background(Colors.White)
                .BorderColor(Color.FromHex(BorderColor))
                .Padding(5)
                .Text(statusText)
                .FontSize(12)
                .FontColor(statusColor);

            textContainer.AlignCenter();
        });
    }

    protected virtual void GenerateKeyValue(IContainer container, string key, string value)
    {
        var _container = Localized.IsAr ? container.AlignRight() : container.AlignLeft();

        _container.Text(text =>
         {
             text.Span(key)
             .FontSize(10)
             .Bold()
             .FontColor(Colors.Grey.Darken1);

             text.Span(value)
             .FontSize(10)
             .FontColor(Colors.Grey.Darken3);
         });
    }

    protected virtual string GetTimeRange(TimeOnly? startTime, TimeOnly? endTime)
    {
        if (startTime == null || endTime == null)
            return "";

        return $"{startTime:HH:mm} - {endTime:HH:mm}";
    }
    
    protected virtual string GetDate(string date)
    {
        return DateTime.Parse(date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    protected void GenerateScheduleTable(IContainer container, List<DailyScheduleDto> schedules)
    {
        container.Border(1).BorderColor(Color.FromHex(BorderColor)).CornerRadius(BorderRadius - 1).Table(table =>
        {
            // Define columns based on language direction
            table.ColumnsDefinition(columns =>
            {
                if (Localized.IsAr)
                {
                    // Arabic: Course, Room, Group, Time (right to left)
                    columns.RelativeColumn(3f);   // Course
                    columns.RelativeColumn(2.5f); // Room/Session
                    columns.RelativeColumn(2.5f); // Group
                    columns.RelativeColumn(2f);   // Time
                }
                else
                {
                    // English: Time, Group, Room, Course (left to right)
                    columns.RelativeColumn(2f);   // Time
                    columns.RelativeColumn(2.5f); // Group
                    columns.RelativeColumn(2.5f); // Room/Session
                    columns.RelativeColumn(3f);   // Course
                }
            });

            // Table Header
            table.Header(header =>
            {
                if (Localized.IsAr)
                {
                    // Arabic order: Course, Room, Group, Time
                    GenerateTableHeaderCell(header.Cell(), Localized.Course);
                    GenerateTableHeaderCell(header.Cell(), Localized.Room);
                    GenerateTableHeaderCell(header.Cell(), Localized.Group);
                    GenerateTableHeaderCell(header.Cell(), Localized.Time);
                }
                else
                {
                    // English order: Time, Group, Room, Course
                    GenerateTableHeaderCell(header.Cell(), Localized.Time);
                    GenerateTableHeaderCell(header.Cell(), Localized.Group);
                    GenerateTableHeaderCell(header.Cell(), Localized.Room);
                    GenerateTableHeaderCell(header.Cell(), Localized.Course);
                }
            });

            // Schedule rows
            foreach (var schedule in schedules.OrderBy(s => s.StartTime))
            {
                if (Localized.IsAr)
                {
                    // Arabic order: Course, Room, Group, Time
                    GenerateTableBodyCell(table.Cell(), schedule.CourseName ?? "");
                    GenerateTableBodyCell(table.Cell(), schedule.RoomName ?? "");
                    GenerateTableBodyCell(table.Cell(), schedule.GroupName ?? "");
                    GenerateTableBodyCell(table.Cell(), GetTimeRange(schedule.StartTime, schedule.EndTime));
                }
                else
                {
                    // English order: Time, Group, Room, Course
                    GenerateTableBodyCell(table.Cell(), GetTimeRange(schedule.StartTime, schedule.EndTime));
                    GenerateTableBodyCell(table.Cell(), schedule.GroupName ?? "");
                    GenerateTableBodyCell(table.Cell(), schedule.RoomName ?? "");
                    GenerateTableBodyCell(table.Cell(), schedule.CourseName ?? "");
                }
            }
        });
    }

}