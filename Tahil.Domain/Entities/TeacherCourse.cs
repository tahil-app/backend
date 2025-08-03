namespace Tahil.Domain.Entities;

public class TeacherCourse : Base
{
    public int TeacherId { get; set; }
    public int CourseId { get; set; }

    public Teacher Teacher { get; set; } = default!;
    public Course Course { get; set; } = default!;
}