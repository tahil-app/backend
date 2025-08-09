namespace Tahil.Domain.Dtos;

public class ClassScheduleLookupsDto
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
}