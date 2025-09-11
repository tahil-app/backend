using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class TeacherScheduleReport: BaseReport, IReport
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IApplicationContext _applicationContext;

    public TeacherScheduleReport(LocalizedStrings localized, ITeacherRepository teacherRepository, IApplicationContext applicationContext) 
        : base(localized)
    {
        _teacherRepository = teacherRepository;
        _applicationContext = applicationContext;   
    }

    public ReportType Type => ReportType.TeacherSchedule;

    public async Task<byte[]> GenerateAsync<T>(T dataSet)
    {
        int teacherId;

        if (dataSet is int id)
            teacherId = id;

        else if (dataSet is TeacherDto teacher)
            teacherId = teacher.Id;

        else
            throw new ArgumentException("Expected int (teacherId) or TeacherDto data", nameof(dataSet));

        return await GenerateTeacherScheduleAsync(teacherId);
    }

    public async Task<byte[]> GenerateTeacherScheduleAsync(int teacherId)
    {
        var teacher = await _teacherRepository.GetAsync(r => r.Id == teacherId && r.User.TenantId == _applicationContext.TenantId);

        if (teacher == null || teacher.Id == 0)
        {
            throw new ArgumentException($"Teacher with ID {teacherId} not found", nameof(teacherId));
        }

        // Get teacher schedules from database
        var teacherSchedules = await _teacherRepository.GetTeacherSchedulesAsync(teacherId, _applicationContext.TenantId);

        var content = new Action<IContainer>(container =>
        {
            container.Column(column =>
            {
                // Teacher Information Section
                column.Item().PaddingBottom(0).Column(teacherInfo =>
                {
                    GenerateKeyValue(teacherInfo.Item(), $"{Localized.Teacher} : ", teacher.User.Name ?? "");

                    if (!string.IsNullOrEmpty(teacher.User.PhoneNumber))
                    {
                        teacherInfo.Item().Height(4);
                        GenerateKeyValue(teacherInfo.Item(), $"{Localized.PhoneNumber} : ", teacher.User.PhoneNumber ?? "");
                    }
                });

                // Weekly Schedule - Organized by Days
                column.Item().PaddingTop(15).Column(scheduleColumn =>
                {
                    var schedulesByDay = teacherSchedules.GroupBy(s => s.Day).OrderBy(g => g.Key);
                    foreach (var dayGroup in schedulesByDay)
                    {
                        // Day Header
                        GenerateDayHeader(scheduleColumn.Item(), dayGroup.Key);

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

    private void GenerateScheduleTable(IContainer container, List<DailyScheduleDto> schedules)
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