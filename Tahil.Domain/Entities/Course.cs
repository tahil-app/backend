using Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class Course : Base
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();

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
