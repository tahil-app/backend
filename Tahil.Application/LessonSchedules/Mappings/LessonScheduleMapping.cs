using Tahil.Domain.Entities;

namespace Tahil.Application.LessonSchedules.Mappings;

public static class LessonScheduleMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<LessonSchedule, LessonScheduleDto>.NewConfig()
            .Map(dest => dest.GroupName, src => src.Group.Name)
            .Map(dest => dest.RoomName, src => src.Room.Name)
            .Map(dest => dest.CourseName, src => src.Course.Name)
            .Map(dest => dest.TeacherName, src => src.Teacher.User.Name);
    }

    public static LessonSchedule ToLessonSchedule(this LessonScheduleDto model)
    {
        return new LessonSchedule
        {
            Id = model.Id,
            RoomId = model.RoomId,
            GroupId = model.GroupId,
            CourseId = model.CourseId,
            TeacherId = model.TeacherId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Day = model.Day,
            StartTime = model.StartTime,
            EndTime = model.EndTime
        };
    }
}