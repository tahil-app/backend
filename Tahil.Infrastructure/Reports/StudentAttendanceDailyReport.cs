using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class StudentAttendanceDailyReport : BaseReport, IReport
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentAttendnceRepository _attendnceRepository;
    private readonly IApplicationContext _applicationContext;

    public StudentAttendanceDailyReport(LocalizedStrings localized, IStudentAttendnceRepository attendnceRepository, IApplicationContext applicationContext, IStudentRepository studentRepository)
        : base(localized)
    {
        _attendnceRepository = attendnceRepository;
        _applicationContext = applicationContext;
        _studentRepository = studentRepository;
    }

    public ReportType Type => ReportType.StudentAttendnceDaily;

    public async Task<byte[]> GenerateAsync<T>(T dataSet)
    {
        int studentId;
        int year;
        int month;

        if (dataSet is { } anonymous &&
                 anonymous.GetType().GetProperty("Id")?.GetValue(anonymous) is int id &&
                 anonymous.GetType().GetProperty("Year")?.GetValue(anonymous) is int y &&
                 anonymous.GetType().GetProperty("Month")?.GetValue(anonymous) is int m)
        {
            studentId = id;
            year = y;
            month = m;
        }
        else
        {
            throw new ArgumentException("Expected StudentAttendanceDailyReportDto or anonymous object with Id, Year properties", nameof(dataSet));
        }

        return await GenerateStudentAttendanceDailyReportAsync(studentId, year, month);
    }

    public async Task<byte[]> GenerateStudentAttendanceDailyReportAsync(int studentId, int year, int month)
    {
        var student = await _studentRepository.GetAsync(r => r.Id == studentId && r.User.TenantId == _applicationContext.TenantId);
        
        if (student == null || student.Id == 0)
        {
            throw new ArgumentException($"Student with ID {studentId} not found", nameof(studentId));
        }

        // Get student monthly attendances from database
        var monthlyAttendancesResult = await _attendnceRepository.GetStudentDailyAttendancesAsync(studentId, year, month, _applicationContext.TenantId);
        var monthlyAttendances = monthlyAttendancesResult.IsSuccess ? monthlyAttendancesResult.Value : new List<StudentDailyAttendanceDto>();

        // Calculate summary statistics
        var totalPresent = monthlyAttendances.Count(m => m.Present);
        var totalAbsent = monthlyAttendances.Count(m => m.Absent);
        var totalLate = monthlyAttendances.Count(m => m.Late);
        var totalSessions = totalPresent + totalAbsent + totalLate;
        var attendancePercentage = totalSessions > 0 ? Math.Round((double)(totalPresent + totalLate) / totalSessions * 100, 1) : 0;

        // Calculate performance analysis
        var bestDay = monthlyAttendances.OrderByDescending(m => m.Present ? 1 : m.Late ? 0.5 : 0).FirstOrDefault();
        var worstDay = monthlyAttendances.OrderBy(m => m.Present ? 1 : m.Late ? 0.5 : 0).FirstOrDefault();
        var overallPerformance = attendancePercentage >= 80 ? Localized.Excellent : attendancePercentage >= 60 ? Localized.Good : Localized.BelowAverage;

        var content = new Action<IContainer>(container =>
        {
            container.Column(column =>
            {
                // Student Information Section
                column.Item().PaddingBottom(10).Column(studentInfo =>
                {
                    GenerateKeyValue(studentInfo.Item(), $"{Localized.Student} : ", student.User.Name ?? "");

                    if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                    {
                        studentInfo.Item().Height(4);
                        GenerateKeyValue(studentInfo.Item(), $"{Localized.PhoneNumber} : ", student.User.PhoneNumber ?? "");
                    }
                });

                // Attendance Summary Section
                column.Item().Element(headerContainer => GenerateTextHeader(headerContainer, text: $"{Localized.AttendanceSummary}: {Localized.GetMonthName(month)} {year}"));
                
                column.Item().Height(10);

                GenerateAttendanceSummary(column.Item(), totalSessions, totalPresent, totalAbsent, totalLate, attendancePercentage, year);

                column.Item().Height(20);

                column.Item().Element(headerContainer => GenerateTextHeader(headerContainer, text: Localized.DailyDetails));

                column.Item().Height(10);

                GenerateDailyDetails(column.Item(), monthlyAttendances);
            
            });
        });

        return GenerateReport(Localized.StudentAttendnceDaily, "", content);
    }

    private void GenerateAttendanceSummary(IContainer container, int total, int present, int absent, int late, double percentage, int year)
    {
        container.Background(Color.FromHex("#f8f9fa"))
            .Border(1).BorderColor(Colors.Grey.Darken1)
            .CornerRadius(BorderRadius)
            .Padding(12)
            .Column(column =>
            {
                // Summary stats in a row
                column.Item().Row(row =>
                {
                    if (Localized.IsAr)
                    {
                        // Arabic order: Percentage, Late, Absent, Present, Total (right to left)
                        
                        // Attendance Percentage
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text($"{percentage}%")
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#007bff"));

                            col.Item().Text(Localized.AttendancePercentage)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Late
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(late.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#fd7e14"));

                            col.Item().Text(Localized.Late)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Absent
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(absent.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#dc3545"));

                            col.Item().Text(Localized.Absent)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Present
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(present.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#28a745"));

                            col.Item().Text(Localized.Present)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Total
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(total.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);

                            col.Item().Text(Localized.Total)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });
                    }
                    else
                    {
                        // English order: Total, Present, Absent, Late, Percentage (left to right)
                        
                        // Total
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(total.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);

                            col.Item().Text(Localized.Total)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Present
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(present.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#28a745"));

                            col.Item().Text(Localized.Present)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Absent
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(absent.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#dc3545"));

                            col.Item().Text(Localized.Absent)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Late
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(late.ToString())
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#fd7e14"));

                            col.Item().Text(Localized.Late)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });

                        // Attendance Percentage
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text($"{percentage}%")
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Color.FromHex("#007bff"));

                            col.Item().Text(Localized.AttendancePercentage)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);
                        });
                    }
                });
            });
    }

    private void GenerateDailyDetails(IContainer container, List<StudentDailyAttendanceDto> dailyAttendances)
    {
        container.Background(Color.FromHex("#f8f9fa"))
            .Column(column =>
            {
                // Daily table
                column.Item().Border(1).BorderColor(Color.FromHex(BorderColor)).CornerRadius(BorderRadius - 1).Table(table =>
                {
                    // Define columns based on language direction
                    table.ColumnsDefinition(columns =>
                    {
                        if (Localized.IsAr)
                        {
                            // Arabic: Absent, Late, Present, Course, Time, Day (right to left)
                            columns.RelativeColumn(1f);   // Absent
                            columns.RelativeColumn(1f);   // Late
                            columns.RelativeColumn(1f);   // Present
                            columns.RelativeColumn(2.5f); // Course
                            columns.RelativeColumn(2f);   // Time
                            columns.RelativeColumn(1.5f); // Day
                        }
                        else
                        {
                            // English: Day, Time, Course, Present, Late, Absent (left to right)
                            columns.RelativeColumn(1.5f); // Day
                            columns.RelativeColumn(2f);   // Time
                            columns.RelativeColumn(2.5f); // Course
                            columns.RelativeColumn(1f);   // Present
                            columns.RelativeColumn(1f);   // Late
                            columns.RelativeColumn(1f);   // Absent
                        }
                    });

                    // Table Header
                    table.Header(header =>
                    {
                        if (Localized.IsAr)
                        {
                            // Arabic order: Absent, Late, Present, Course, Time, Day
                            GenerateTableHeaderCell(header.Cell(), Localized.Absent, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Late, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Present, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Course);
                            GenerateTableHeaderCell(header.Cell(), Localized.Time);
                            GenerateTableHeaderCell(header.Cell(), "اليوم"); // Day in Arabic
                        }
                        else
                        {
                            // English order: Day, Time, Course, Present, Late, Absent
                            GenerateTableHeaderCell(header.Cell(), "Day");
                            GenerateTableHeaderCell(header.Cell(), Localized.Time);
                            GenerateTableHeaderCell(header.Cell(), Localized.Course);
                            GenerateTableHeaderCell(header.Cell(), Localized.Present, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Late, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Absent, true);
                        }
                    });

                    // Generate rows for each daily attendance
                    foreach (var attendance in dailyAttendances.OrderBy(a => a.Date).ThenBy(a => a.StartTime))
                    {
                        var timeRange = GetTimeRange(attendance.StartTime, attendance.EndTime);
                        var dateStr = GetDate(attendance.Date.ToString());

                        if (Localized.IsAr)
                        {
                            // Arabic order: Absent, Late, Present, Course, Time, Day
                            GenerateAttendanceStatusCell(table.Cell(), false, false, attendance.Absent);
                            GenerateAttendanceStatusCell(table.Cell(), false, attendance.Late, false);
                            GenerateAttendanceStatusCell(table.Cell(), attendance.Present, false, false);
                            GenerateTableBodyCell(table.Cell(), attendance.CourseName ?? "");
                            GenerateTableBodyCell(table.Cell(), timeRange);
                            GenerateTableBodyCell(table.Cell(), dateStr);
                        }
                        else
                        {
                            // English order: Day, Time, Course, Present, Late, Absent
                            GenerateTableBodyCell(table.Cell(), dateStr);
                            GenerateTableBodyCell(table.Cell(), timeRange);
                            GenerateTableBodyCell(table.Cell(), attendance.CourseName ?? "");
                            GenerateAttendanceStatusCell(table.Cell(), attendance.Present, false, false);
                            GenerateAttendanceStatusCell(table.Cell(), false, attendance.Late, false);
                            GenerateAttendanceStatusCell(table.Cell(), false, false, attendance.Absent);
                        }
                    }
                });
            });
    }

}