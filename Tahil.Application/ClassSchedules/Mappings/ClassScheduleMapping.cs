using Tahil.Domain.Entities;

namespace Tahil.Application.ClassSchedules.Mappings;

public static class ClassScheduleMapping
{
    public static void RegisterMappings()
    {
        //TypeAdapterConfig<ClassSchedule, ClassScheduleDto>.NewConfig()
        //    .Map(dest => dest.GroupName, src => src.Group.Name)
        //    .Map(dest => dest.RoomName, src => src.Room.Name)
        //    .Map(dest => dest.CourseName, src => src.Group.Course.Name)
        //    .Map(dest => dest.TeacherName, src => src.Group.Teacher.User.Name);
    }

    public static ClassSchedule ToClassSchedule(this ClassScheduleDto model)
    {
        return new ClassSchedule
        {
            RoomId = model.RoomId,
            GroupId = model.GroupId,
            Color = model.Color,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Day = model.Day,
            StartTime = model.StartTime,
            EndTime = model.EndTime
        };
    }
}