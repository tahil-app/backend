using Tahil.Common.Exceptions;

namespace Tahil.Infrastructure.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(BEContext context) : base(context.Set<Room>())
    {
    }

    public async Task AddRoomAsync(Room room)
    {
        await CheckExistsAsync(room);

        room.IsActive = true;
        Add(room);
    }

    private async Task CheckExistsAsync(Room room) 
    {
        var existRoom = await GetAsync(u => u.Name == room.Name);

        if (existRoom is not null && existRoom.Name == room.Name)
            throw new DuplicateException("Room");
    }
}