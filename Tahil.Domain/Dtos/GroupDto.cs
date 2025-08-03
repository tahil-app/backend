namespace Tahil.Domain.Dtos;

public class GroupDto: BaseDto
{
    public string Name { get; set; } = default!;
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public bool IsActive { get; set; }

    public int NumberOfStudents { get; set; }
}