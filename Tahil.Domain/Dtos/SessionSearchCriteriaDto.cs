namespace Tahil.Domain.Dtos;

public class SessionSearchCriteriaDto
{
    public int? CourseId { get; set; }
    public int? GroupId { get; set; }
    public int? RoomId { get; set; }
    
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    public ClassSessionStatus? Status { get; set; }
}