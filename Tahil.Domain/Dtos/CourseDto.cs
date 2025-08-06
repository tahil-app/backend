namespace Tahil.Domain.Dtos;

public class CourseDto: BaseDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public bool IsActive { get; set; }
}