using Tahil.Common.Helpers;

namespace Tahil.Domain.Entities;

public class Group : Base
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }

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
