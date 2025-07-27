namespace Tahil.Domain.Entities;

public class Course : Base
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public void Update(CourseDto courseDto) 
    {
        Name = courseDto.Name;
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
