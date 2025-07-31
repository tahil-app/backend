namespace Tahil.Domain.Entities;

public class Group : Base
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }

    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();

    public void Validate()
    {
        Check.IsNull(Name, nameof(Name));
        Check.IsPositive(Capacity, nameof(Capacity));
    }

    public void Update(GroupDto groupDto)
    {
        Name = groupDto.Name;
        Capacity = groupDto.Capacity;

        Validate();
    }
}
