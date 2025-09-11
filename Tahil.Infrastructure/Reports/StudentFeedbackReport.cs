using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;


namespace Tahil.Infrastructure.Reports;

public class StudentFeedbackReport : BaseReport, IReport
{
    private readonly IStudentRepository _studentRepository;
    private readonly IApplicationContext _applicationContext;

    public StudentFeedbackReport(LocalizedStrings localized, IStudentRepository studentRepository, IApplicationContext applicationContext)
        : base(localized)
    {
        _studentRepository = studentRepository;
        _applicationContext = applicationContext;
    }

    public ReportType Type => ReportType.StudentFeedback;

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
            throw new ArgumentException("Expected StudentFeedbackReportDto or anonymous object with Id, Year, Month properties", nameof(dataSet));
        }

        return await GenerateStudentScheduleAsync(studentId, year, month);
    }

    public async Task<byte[]> GenerateStudentScheduleAsync(int studentId, int year, int month)
    {
        var student = await _studentRepository.GetAsync(r => r.Id == studentId && r.User.TenantId == _applicationContext.TenantId);

        if (student == null || student.Id == 0)
        {
            throw new ArgumentException($"Student with ID {studentId} not found", nameof(studentId));
        }

        // Get student feedbacks from database
        var studentFeedbacks = await _studentRepository.GetStudentFeedbacksAsync(studentId, year, month, _applicationContext.TenantId);

        var content = new Action<IContainer>(container =>
        {
            container.Column(column =>
            {
                // Student Information Section
                column.Item().PaddingBottom(0).Column(studentInfo =>
                {
                    GenerateKeyValue(studentInfo.Item(), $"{Localized.Student} : ", student.User.Name ?? "");

                    if (!string.IsNullOrEmpty(student.User.PhoneNumber))
                    {
                        studentInfo.Item().Height(4);
                        GenerateKeyValue(studentInfo.Item(), $"{Localized.PhoneNumber} : ", student.User.PhoneNumber ?? "");
                    }
                });

                // Feedbacks
                column.Item().PaddingTop(15).Column(scheduleColumn =>
                {
                    // Header
                    GenerateTextHeader(scheduleColumn.Item(), text: Localized.Comments);

                    foreach (var feedback in studentFeedbacks)
                    {
                        scheduleColumn.Item().PaddingVertical(10).Element(container => 
                        {
                            container.Border(1).BorderColor(Color.FromHex(BorderColor)).CornerRadius(BorderRadius - 1).Table(table =>
                            {
                                // Define columns for feedback: Date, Teacher, Comment
                                table.ColumnsDefinition(columns =>
                                {
                                    if (Localized.IsAr)
                                    {
                                        // Arabic: Date, Teacher, Comment (right to left)
                                        columns.RelativeColumn(5f);   // Comment
                                        columns.RelativeColumn(3f);   // Teacher
                                        columns.RelativeColumn(2f);   // Date
                                    }
                                    else
                                    {
                                        // English: Date, Teacher, Comment (left to right)
                                        columns.RelativeColumn(2f);   // Date
                                        columns.RelativeColumn(3f);   // Teacher
                                        columns.RelativeColumn(5f);   // Comment
                                    }
                                });

                                // Table Header
                                table.Header(header =>
                                {
                                    if (Localized.IsAr)
                                    {
                                        // Arabic order: Date, Teacher, Comment
                                        GenerateTableHeaderCell(header.Cell(), Localized.Comments);
                                        GenerateTableHeaderCell(header.Cell(), Localized.Teacher);
                                        GenerateTableHeaderCell(header.Cell(), Localized.Date);
                                    }
                                    else
                                    {
                                        // English order: Date, Teacher, Comment
                                        GenerateTableHeaderCell(header.Cell(), Localized.Date);
                                        GenerateTableHeaderCell(header.Cell(), Localized.Teacher);
                                        GenerateTableHeaderCell(header.Cell(), Localized.Comments);
                                    }
                                });

                                // Feedback rows
                                if (Localized.IsAr)
                                {
                                    // Arabic order: Date, Teacher, Comment
                                    GenerateTableBodyCell(table.Cell(), feedback.Comment ?? "");
                                    GenerateTableBodyCell(table.Cell(), feedback.Name ?? "");
                                    GenerateTableBodyCell(table.Cell(), GetDate(feedback.Date ?? ""));
                                }
                                else
                                {
                                    // English order: Date, Teacher, Comment
                                    GenerateTableBodyCell(table.Cell(), GetDate(feedback.Date ?? ""));
                                    GenerateTableBodyCell(table.Cell(), feedback.Name ?? "");
                                    GenerateTableBodyCell(table.Cell(), feedback.Comment ?? "");
                                }
                            });
                        });
                    }

                });

            });
        });

        return GenerateReport(Localized.Comments, "", content);
    }

}