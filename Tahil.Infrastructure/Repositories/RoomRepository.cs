using Tahil.Common.Exceptions;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public RoomRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Room>())
    {
        _localizedStrings = localizedStrings;
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
            throw new DuplicateException(_localizedStrings.DuplicatedRoom);
    }
}