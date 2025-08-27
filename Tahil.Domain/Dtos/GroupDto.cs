namespace Tahil.Domain.Dtos;

public class GroupDto: BaseDto
{
    public string Name { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; }

    public int? NumberOfStudents { get; set; }
    public LookupDto? Course { get; set; }
    public LookupDto? Teacher { get; set; }
    public List<LookupDto>? Students { get; set; }
}

public class GroupDailyAttendance 
{
    public int SessionId { get; set; }
    public DateOnly Date { get; set; }
    public int Present { get; set; }
    public int Late { get; set; }
    public int Absent { get; set; }
}