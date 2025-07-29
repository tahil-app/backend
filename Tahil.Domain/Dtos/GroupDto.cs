namespace Tahil.Domain.Dtos;

public class GroupDto: BaseDto
{
    public string Name { get; set; } = default!;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
}