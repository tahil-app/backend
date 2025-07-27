namespace Tahil.Domain.Entities;

public class Room : Base
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public void Update(RoomDto roomDto) 
    {
        Name = roomDto.Name;
    }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
