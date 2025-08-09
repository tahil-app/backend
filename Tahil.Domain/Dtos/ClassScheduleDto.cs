namespace Tahil.Domain.Dtos;

public class ClassScheduleDto: BaseDto
{
    public int RoomId { get; set; }
    public int GroupId { get; set; }
    
    public WeekDays Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ClassScheduleStatus Status { get; set; }
}