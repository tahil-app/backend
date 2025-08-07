using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public RoomRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Room>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task<Result<bool>> AddRoomAsync(Room room, Guid tenantId)
    {
        var result = await CheckDuplicateRoomNameAsync(room, tenantId);

        if (result.IsSuccess)
        {
            room.TenantId = tenantId;
            room.IsActive = true;
            Add(room);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteRoomAsync(int id, Guid tenantId)
    {
        var room = await GetAsync(r => r.Id == id && r.TenantId == tenantId, [r => r.Schedules, r => r.Sessions]);

        if (room is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableRoom);

        // Check if room has child relationships
        if (room.Schedules.Any())
            return Result<bool>.Failure(_localizedStrings.RoomHasSchedules);

        if (room.Sessions.Any())
            return Result<bool>.Failure(_localizedStrings.RoomHasSessions);

        // If no child relationships exist, proceed with deletion
        HardDelete(room);
        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckDuplicateRoomNameAsync(Room room, Guid tenantId) 
    {
        var existRoom = await GetAsync(r => r.Name == room.Name && r.TenantId == tenantId);

        // Check if room name is duplicated
        if (existRoom is not null && existRoom.Name == room.Name)
            return Result<bool>.Failure(_localizedStrings.DuplicatedRoom);

        return Result<bool>.Success(true);
    }

}