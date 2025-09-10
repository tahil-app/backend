using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Reports;

public class TeacherScheduleReport(LocalizedStrings localized) : BaseReport, IReport
{
    public ReportType Type => ReportType.TeacherSchedule;

    public async Task<byte[]> GenerateAsync<T>(T dataSet) where T : BaseDto
    {
        if (dataSet is not TeacherDto teacher)
        {
            throw new ArgumentException("Expected TeacherDto data", nameof(dataSet));
        }

        teacher = new TeacherDto
        {
            Name = "علي رضا الموسوي",
            Email = "ali.mousawi@tahil.edu",
            PhoneNumber = "+966 50 123 4567",
            Experience = "10 years of teaching experience in Mathematics and Physics",
            Qualification = "Master's Degree in Mathematics, PhD in Physics",
            Courses = new List<CourseDto>
            {
                new CourseDto { Name = "Advanced Mathematics", Description = "Advanced calculus and algebra" },
                new CourseDto { Name = "Physics Fundamentals", Description = "Basic physics principles and applications" },
                new CourseDto { Name = "Mathematical Analysis", Description = "Real analysis and complex analysis" },
                new CourseDto { Name = "Quantum Mechanics", Description = "Introduction to quantum physics" },
                new CourseDto { Name = "Linear Algebra", Description = "Vector spaces and linear transformations" }
            }
        };

        var content = new Action<IContainer>(container =>
        {
            container.Column(column =>
            {
                // Teacher Information Section
                column.Item().PaddingBottom(15).Column(teacherInfo =>
                {
                    teacherInfo.Item().Text($"{localized.Teacher} {localized.Name}")
                        .FontSize(14)
                        .Bold()
                        .FontColor(Colors.Blue.Darken2);
                    teacherInfo.Item().PaddingTop(5).Text(teacher.Name ?? "")
                        .FontSize(12)
                        .FontColor(Colors.Grey.Darken3);

                    if (!string.IsNullOrEmpty(teacher.Email))
                    {
                        teacherInfo.Item().PaddingTop(2).Text($"{localized.Email}: {teacher.Email}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    }

                    if (!string.IsNullOrEmpty(teacher.PhoneNumber))
                    {
                        teacherInfo.Item().PaddingTop(2).Text($"{localized.PhoneNumber}: {teacher.PhoneNumber}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    }

                    if (!string.IsNullOrEmpty(teacher.Experience))
                    {
                        teacherInfo.Item().PaddingTop(5).Text("Experience:")
                            .FontSize(11)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        teacherInfo.Item().PaddingTop(2).Text(teacher.Experience)
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                    }

                    if (!string.IsNullOrEmpty(teacher.Qualification))
                    {
                        teacherInfo.Item().PaddingTop(5).Text("Qualification:")
                            .FontSize(11)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        teacherInfo.Item().PaddingTop(2).Text(teacher.Qualification)
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                    }
                });

                // Schedule Table
                column.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1.5f); // Day
                        columns.RelativeColumn(2f);   // Time
                        columns.RelativeColumn(2.5f); // Course
                        columns.RelativeColumn(2f);   // Room
                        columns.RelativeColumn(2f);   // Group
                    });

                    // Table Header
                    table.Header(header =>
                    {
                        header.Cell().Element(container => CellStyle(container).Text(GetDayHeader()).FontSize(10).Bold());
                        header.Cell().Element(container => CellStyle(container).Text(GetTimeHeader()).FontSize(10).Bold());
                        header.Cell().Element(container => CellStyle(container).Text(localized.Course).FontSize(10).Bold());
                        header.Cell().Element(container => CellStyle(container).Text(localized.Room).FontSize(10).Bold());
                        header.Cell().Element(container => CellStyle(container).Text(localized.Group).FontSize(10).Bold());
                    });

                    // Sample schedule data - In real implementation, this would come from the teacher's actual schedule
                    var sampleSchedules = GetSampleScheduleData();

                    foreach (var schedule in sampleSchedules)
                    {
                        table.Cell().Element(container => CellStyle(container).Text(GetDayName(schedule.Day)).FontSize(9));
                        table.Cell().Element(container => CellStyle(container).Text(GetTimeRange(schedule.StartTime, schedule.EndTime)).FontSize(9));
                        table.Cell().Element(container => CellStyle(container).Text(schedule.CourseName ?? "").FontSize(9));
                        table.Cell().Element(container => CellStyle(container).Text(schedule.RoomName ?? "").FontSize(9));
                        table.Cell().Element(container => CellStyle(container).Text(schedule.GroupName ?? "").FontSize(9));
                    }
                });

                // Additional Information
                if (teacher.Courses?.Any() == true)
                {
                    column.Item().PaddingTop(20).Column(coursesInfo =>
                    {
                        coursesInfo.Item().Text($"{localized.Course}s")
                            .FontSize(12)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        foreach (var course in teacher.Courses.Take(5)) // Limit to 5 courses
                        {
                            coursesInfo.Item().PaddingTop(3).Text($"• {course.Name}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);
                        }

                        if (teacher.Courses.Count > 5)
                        {
                            coursesInfo.Item().PaddingTop(3).Text($"... and {teacher.Courses.Count - 5} more courses")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        }
                    });
                }
            });
        });

        var report = GenerateReport(localized, localized.Schedules, teacher.Name ?? "", content);
        return report;
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Background(Colors.White);
    }

    private string GetDayHeader() => localized.IsAr ? "اليوم" : "Day";
    private string GetTimeHeader() => localized.IsAr ? "الوقت" : "Time";

    private string GetDayName(WeekDays day) => localized.IsAr ? GetArabicDayName(day) : day.ToString();

    private string GetArabicDayName(WeekDays day) => day switch
    {
        WeekDays.Saturday => "السبت",
        WeekDays.Sunday => "الأحد",
        WeekDays.Monday => "الاثنين",
        WeekDays.Tuesday => "الثلاثاء",
        WeekDays.Wednesday => "الأربعاء",
        WeekDays.Thursday => "الخميس",
        WeekDays.Friday => "الجمعة",
        _ => day.ToString()
    };

    private string GetTimeRange(TimeOnly? startTime, TimeOnly? endTime)
    {
        if (startTime == null || endTime == null)
            return "";

        return $"{startTime:HH:mm} - {endTime:HH:mm}";
    }

    private List<DailyScheduleDto> GetSampleScheduleData()
    {
        // This is sample data - in real implementation, this would come from the database
        return new List<DailyScheduleDto>
        {
            new() { Day = WeekDays.Saturday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Advanced Mathematics", RoomName = "Math Lab 201", GroupName = "Advanced Math Group A" },
            new() { Day = WeekDays.Saturday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Physics Fundamentals", RoomName = "Physics Lab 102", GroupName = "Physics Group B" },
            new() { Day = WeekDays.Saturday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Linear Algebra", RoomName = "Math Lab 201", GroupName = "Linear Algebra Group C" },
            
            new() { Day = WeekDays.Sunday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Mathematical Analysis", RoomName = "Math Lab 201", GroupName = "Analysis Group A" },
            new() { Day = WeekDays.Sunday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Quantum Mechanics", RoomName = "Physics Lab 102", GroupName = "Quantum Group B" },
            new() { Day = WeekDays.Sunday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Advanced Mathematics", RoomName = "Math Lab 201", GroupName = "Advanced Math Group B" },
            
            new() { Day = WeekDays.Monday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Physics Fundamentals", RoomName = "Physics Lab 102", GroupName = "Physics Group A" },
            new() { Day = WeekDays.Monday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Linear Algebra", RoomName = "Math Lab 201", GroupName = "Linear Algebra Group A" },
            new() { Day = WeekDays.Monday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Mathematical Analysis", RoomName = "Math Lab 201", GroupName = "Analysis Group B" },
            
            new() { Day = WeekDays.Tuesday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Quantum Mechanics", RoomName = "Physics Lab 102", GroupName = "Quantum Group A" },
            new() { Day = WeekDays.Tuesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Advanced Mathematics", RoomName = "Math Lab 201", GroupName = "Advanced Math Group C" },
            new() { Day = WeekDays.Tuesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Physics Fundamentals", RoomName = "Physics Lab 102", GroupName = "Physics Group C" },
            
            new() { Day = WeekDays.Wednesday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Linear Algebra", RoomName = "Math Lab 201", GroupName = "Linear Algebra Group B" },
            new() { Day = WeekDays.Wednesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Mathematical Analysis", RoomName = "Math Lab 201", GroupName = "Analysis Group C" },
            new() { Day = WeekDays.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Quantum Mechanics", RoomName = "Physics Lab 102", GroupName = "Quantum Group C" },
            
            new() { Day = WeekDays.Thursday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(9, 30), CourseName = "Advanced Mathematics", RoomName = "Math Lab 201", GroupName = "Advanced Math Group A" },
            new() { Day = WeekDays.Thursday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 30), CourseName = "Physics Fundamentals", RoomName = "Physics Lab 102", GroupName = "Physics Group B" },
            new() { Day = WeekDays.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), CourseName = "Linear Algebra", RoomName = "Math Lab 201", GroupName = "Linear Algebra Group C" }
        };
    }
}