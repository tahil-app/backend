using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class StudentAttendanceMonthlyReport : BaseReport, IReport
{
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentAttendnceRepository _attendnceRepository;
    private readonly IApplicationContext _applicationContext;

    public StudentAttendanceMonthlyReport(LocalizedStrings localized, IStudentAttendnceRepository attendnceRepository, IApplicationContext applicationContext, IStudentRepository studentRepository)
        : base(localized)
    {
        _attendnceRepository = attendnceRepository;
        _applicationContext = applicationContext;
        _studentRepository = studentRepository;
    }

    public ReportType Type => ReportType.StudentAttendnceMonthly;

    public async Task<byte[]> GenerateAsync<T>(T dataSet)
    {
        int studentId;
        int year;

        if (dataSet is { } anonymous &&
                 anonymous.GetType().GetProperty("Id")?.GetValue(anonymous) is int id &&
                 anonymous.GetType().GetProperty("Year")?.GetValue(anonymous) is int y)
        {
            studentId = id;
            year = y;
        }
        else
        {
            throw new ArgumentException("Expected StudentAttendanceMonthlyReportDto or anonymous object with Id, Year properties", nameof(dataSet));
        }

        return await GenerateStudentAttendanceMonthlyReportAsync(studentId, year);
    }

    public async Task<byte[]> GenerateStudentAttendanceMonthlyReportAsync(int studentId, int year)
    {
        var student = await _studentRepository.GetAsync(r => r.Id == studentId && r.User.TenantId == _applicationContext.TenantId);
        
        if (student == null || student.Id == 0)
        {
            throw new ArgumentException($"Student with ID {studentId} not found", nameof(studentId));
        }

        // Get student monthly attendances from database
        var monthlyAttendancesResult = await _attendnceRepository.GetStudentMonthlyAttendancesAsync(studentId, year, _applicationContext.TenantId);
        var monthlyAttendances = monthlyAttendancesResult.IsSuccess ? monthlyAttendancesResult.Value : new List<StudentMonthlyAttendanceDto>();

        // Calculate summary statistics
        var totalPresent = monthlyAttendances.Sum(m => m.Present);
        var totalAbsent = monthlyAttendances.Sum(m => m.Absent);
        var totalLate = monthlyAttendances.Sum(m => m.Late);
        var totalSessions = totalPresent + totalAbsent + totalLate;
        var attendancePercentage = totalSessions > 0 ? Math.Round((double)(totalPresent + totalLate) / totalSessions * 100, 1) : 0;

        // Calculate performance analysis
        var bestMonth = monthlyAttendances.OrderByDescending(m => m.Present + m.Late).FirstOrDefault();
        var worstMonth = monthlyAttendances.OrderBy(m => m.Present + m.Late).FirstOrDefault();
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
                column.Item().Element(headerContainer => GenerateTextHeader(headerContainer, text: $"{Localized.AttendanceSummary}: {year}"));
                
                column.Item().Height(10);

                GenerateAttendanceSummary(column.Item(), totalSessions, totalPresent, totalAbsent, totalLate, attendancePercentage, year);

                column.Item().Height(20);

                column.Item().Element(headerContainer => GenerateTextHeader(headerContainer, text: Localized.MonthlyDetails));

                column.Item().Height(10);

                GenerateMonthlyDetails(column.Item(), monthlyAttendances);

                column.Item().Height(20);

                column.Item().Element(headerContainer => GenerateTextHeader(headerContainer, text: Localized.PerformanceAnalysis));

                column.Item().Height(10);
                
                GeneratePerformanceAnalysis(column.Item(), overallPerformance, bestMonth, worstMonth);
                
            });
        });

        return GenerateReport(Localized.StudentAttendnceMonthly, "", content);
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

    private void GenerateMonthlyDetails(IContainer container, List<StudentMonthlyAttendanceDto> monthlyAttendances)
    {
        container.Background(Color.FromHex("#f8f9fa"))
            .Column(column =>
            {
                // Monthly table
                column.Item().Border(1).BorderColor(Color.FromHex(BorderColor)).CornerRadius(BorderRadius - 1).Table(table =>
                {
                    // Define columns based on language direction
                                table.ColumnsDefinition(columns =>
                                {
                                    if (Localized.IsAr)
                                    {
                            // Arabic: Percentage, Total, Absent, Late, Present, Month (right to left)
                            columns.RelativeColumn(2f);   // Percentage
                            columns.RelativeColumn(1.5f); // Total
                            columns.RelativeColumn(1.5f); // Absent
                            columns.RelativeColumn(1.5f); // Late
                            columns.RelativeColumn(1.5f); // Present
                            columns.RelativeColumn(2f);   // Month
                                    }
                                    else
                                    {
                            // English: Month, Present, Late, Absent, Total, Percentage (left to right)
                            columns.RelativeColumn(2f);   // Month
                            columns.RelativeColumn(1.5f); // Present
                            columns.RelativeColumn(1.5f); // Late
                            columns.RelativeColumn(1.5f); // Absent
                            columns.RelativeColumn(1.5f); // Total
                            columns.RelativeColumn(2f);   // Percentage
                                    }
                                });

                                // Table Header
                                table.Header(header =>
                                {
                                    if (Localized.IsAr)
                                    {
                            // Arabic order: Percentage, Total, Absent, Late, Present, Month
                            GenerateTableHeaderCell(header.Cell(), Localized.AttendancePercentage, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Total, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Absent, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Late, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Present, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Month);
                                    }
                                    else
                                    {
                            // English order: Month, Present, Late, Absent, Total, Percentage
                            GenerateTableHeaderCell(header.Cell(), Localized.Month);
                            GenerateTableHeaderCell(header.Cell(), Localized.Present, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Late, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Absent, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.Total, true);
                            GenerateTableHeaderCell(header.Cell(), Localized.AttendancePercentage, true);
                        }
                    });

                    // Generate rows for all 12 months
                    for (int month = 1; month <= 12; month++)
                    {
                        var monthData = monthlyAttendances.FirstOrDefault(m => m.Month == month);
                        var present = monthData?.Present ?? 0;
                        var late = monthData?.Late ?? 0;
                        var absent = monthData?.Absent ?? 0;
                        var total = present + late + absent;
                        var percentage = total > 0 ? Math.Round((double)(present + late) / total * 100, 1) : 0;

                                if (Localized.IsAr)
                                {
                            // Arabic order: Percentage, Total, Absent, Late, Present, Month
                            GenerateTableBodyCell(table.Cell(), total > 0 ? $"{percentage}" : "", true);
                            GenerateTableBodyCell(table.Cell(), total.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), absent.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), late.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), present.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), Localized.GetMonthName(month));
                                }
                                else
                                {
                            // English order: Month, Present, Late, Absent, Total, Percentage
                            GenerateTableBodyCell(table.Cell(), Localized.GetMonthName(month));
                            GenerateTableBodyCell(table.Cell(), present.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), late.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), absent.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), total.ToString(), true);
                            GenerateTableBodyCell(table.Cell(), total > 0 ? $"{percentage}" : "", true);
                        }
                                }
                            });
                        });
                    }

    private void GeneratePerformanceAnalysis(IContainer container, string overallPerformance, StudentMonthlyAttendanceDto? bestMonth, StudentMonthlyAttendanceDto? worstMonth)
    {
        container.Background(Color.FromHex("#f8f9fa"))
            .Border(1).BorderColor(Colors.Grey.Darken1)
            .CornerRadius(BorderRadius)
            .Padding(12)
            .Column(column =>
            {
                // Performance details
                column.Item().Row(row =>
                {
                    if (Localized.IsAr)
                    {
                        // Arabic order: Best Month, Worst Month, Overall Performance (right to left)
                        
                        // Best Month
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.BestMonth)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(bestMonth != null ? Localized.GetMonthName(bestMonth.Month) : Localized.NoData)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);
                        });

                        // Worst Month
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.WorstMonth)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(worstMonth != null ? Localized.GetMonthName(worstMonth.Month) : Localized.NoData)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);
                        });

                        // Overall Performance
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.OverallPerformance)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(overallPerformance)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(overallPerformance == Localized.Excellent ? Color.FromHex("#28a745") : 
                                          overallPerformance == Localized.Good ? Color.FromHex("#007bff") : 
                                          Color.FromHex("#fd7e14"));
                        });
                    }
                    else
                    {
                        // English order: Overall Performance, Worst Month, Best Month (left to right)
                        
                        // Overall Performance
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.OverallPerformance)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(overallPerformance)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(overallPerformance == Localized.Excellent ? Color.FromHex("#28a745") : 
                                          overallPerformance == Localized.Good ? Color.FromHex("#007bff") : 
                                          Color.FromHex("#fd7e14"));
                        });

                        // Worst Month
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.WorstMonth)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(worstMonth != null ? Localized.GetMonthName(worstMonth.Month) : Localized.NoData)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);
                        });

                        // Best Month
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().PaddingBottom(3).Text(Localized.BestMonth)
                                .FontSize(10)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken1);

                            col.Item().Text(bestMonth != null ? Localized.GetMonthName(bestMonth.Month) : Localized.NoData)
                                .FontSize(12)
                                .Bold()
                                .AlignCenter()
                                .FontColor(Colors.Grey.Darken3);
                        });
                    }
                });
            });
    }


}