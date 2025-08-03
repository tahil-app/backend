namespace Tahil.Domain.Dtos;

public class LessonScheduleLookupsDto
{
    public List<RoomDto> Rooms { get; set; } = new();
    public List<GroupDto> Groups { get; set; } = new();
    public List<CourseTeachers> CourseTeachers { get; set; } = new();
}

public class CourseTeachers 
{
    public CourseDto Course { get; set; }
    public List<TeacherDto> Teachers { get; set; } = new();
}