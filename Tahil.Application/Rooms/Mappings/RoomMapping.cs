using Tahil.Domain.Entities;

namespace Tahil.Application.Rooms.Mappings;

public static class RoomMapping
{
    public static void RegisterMappings()
    {

    }

    public static Room ToRoom(this RoomDto model)
    {
        return new Room
        {
            Name = model.Name,
            IsActive = model.IsActive,
        };
    }
}
