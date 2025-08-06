namespace Tahil.Domain.Dtos;

public class GroupDto: BaseDto
{
    public string Name { get; set; } = default!;
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public bool IsActive { get; set; }
    public int Capacity { get; set; }

    public int? NumberOfStudents { get; set; }
    public CourseDto? Course { get; set; }
    public TeacherDto? Teacher { get; set; }
    public List<StudentDto>? Students { get; set; }
}