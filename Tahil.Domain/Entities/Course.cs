using Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class Course : Base
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public void Validate()
    {
        Check.IsNull(Name, nameof(Name));
    }

    public void Update(CourseDto courseDto) 
    {
        Name = courseDto.Name;

        Validate();
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
