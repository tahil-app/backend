namespace Tahil.Domain.Dtos;

public class StudentMonthlyAttendanceDto
{
    public int Month { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Late { get; set; }
}