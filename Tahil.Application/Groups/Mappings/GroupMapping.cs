using Tahil.Domain.Entities;

namespace Tahil.Application.Groups.Mappings;

public static class GroupMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Group, GroupDto>.NewConfig()
            .Map(dest => dest.NumberOfStudents, src => src.StudentGroups.Count);
    }

    public static Group ToGroup(this GroupDto model)
    {
        return new Group
        {
            Name = model.Name,
            CourseId = model.CourseId,
            TeacherId = model.TeacherId,
        };
    }
}
