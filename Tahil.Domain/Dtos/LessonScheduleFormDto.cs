namespace Tahil.Domain.Dtos;

public class LessonScheduleFormDto
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int RoomId { get; set; }
    public int CourseId { get; set; }
    public int TeacherId { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public List<LessonDailySchedule> DailySchedules { get; set; } = new();
}

public class LessonDailySchedule 
{
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}