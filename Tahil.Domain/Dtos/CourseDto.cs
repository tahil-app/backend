namespace Tahil.Domain.Dtos;

public class CourseDto: BaseDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public bool IsActive { get; set; }

    public int? NumberOfTeachers { get; set; }
    public List<LookupDto>? Teachers { get; set; }
    public List<LookupDto>? Groups { get; set; }
}