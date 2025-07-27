namespace Tahil.Domain.Dtos;

public class RoomDto: BaseDto
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
}