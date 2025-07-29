using Tahil.Domain.Entities;

namespace Tahil.Application.Groups.Mappings;

public static class GroupMapping
{
    public static void RegisterMappings()
    {

    }

    public static Group ToGroup(this GroupDto model)
    {
        return new Group
        {
            Name = model.Name,
            Capacity = model.Capacity
        };
    }
}
