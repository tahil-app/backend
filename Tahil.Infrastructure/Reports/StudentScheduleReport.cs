using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class StudentScheduleReport: BaseReport, IReport
{
    private readonly IStudentRepository _studentRepository;
    private readonly IApplicationContext _applicationContext;

    public StudentScheduleReport(LocalizedStrings localized, IStudentRepository studentRepository, IApplicationContext applicationContext) 
        : base(localized)
    {
        _studentRepository = studentRepository;
        _applicationContext = applicationContext;   
    }

    public ReportType Type => ReportType.StudentSchedule;

    public async Task<byte[]> GenerateAsync<T>(T dataSet)
    {
        int studentId;

        if (dataSet is int id)
            studentId = id;

        else if (dataSet is StudentDto student)
            studentId = student.Id;

        else
            throw new ArgumentException("Expected int (studentId) or StudentDto data", nameof(dataSet));

        return await GenerateStudentScheduleAsync(studentId);
    }

    public async Task<byte[]> GenerateStudentScheduleAsync(int studentId)
    {
        var student = await _studentRepository.GetAsync(r => r.Id == studentId && r.User.TenantId == _applicationContext.TenantId);

        if (student == null || student.Id == 0)
        {
            throw new ArgumentException($"Student with ID {studentId} not found", nameof(studentId));
        }

        // Get student schedules from database
        var studentSchedules = await _studentRepository.GetStudentSchedulesAsync(studentId, _applicationContext.TenantId);

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

                // Weekly Schedule - Organized by Days
                column.Item().PaddingTop(15).Column(scheduleColumn =>
                {
                    var schedulesByDay = studentSchedules.GroupBy(s => s.Day).OrderBy(g => g.Key);
                    foreach (var dayGroup in schedulesByDay)
                    {
                        // Day Header
                        GenerateTextHeader(scheduleColumn.Item(), dayGroup.Key);

                        // Day Schedule Table
                        scheduleColumn.Item().PaddingVertical(10).Element(container => 
                            GenerateScheduleTable(container, dayGroup.ToList()));

                        scheduleColumn.Item().Height(10);
                    }

                });

            });
        });

        return GenerateReport(Localized.Schedules, "", content);
    }

}