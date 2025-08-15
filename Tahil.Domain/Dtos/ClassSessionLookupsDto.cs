namespace Tahil.Domain.Dtos;

public class ClassSessionLookupsDto
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
    public List<CourseDto> Courses { get; set; } = new();
    public List<TeacherDto> Teachers { get; set; } = new();
}