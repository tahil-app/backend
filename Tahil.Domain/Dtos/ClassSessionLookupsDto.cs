namespace Tahil.Domain.Dtos;

public class ClassSessionLookupsDto
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<TeacherDto> Teachers { get; set; } = new();
}