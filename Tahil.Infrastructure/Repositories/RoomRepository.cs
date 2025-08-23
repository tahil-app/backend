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
        // Check if room exists and has any child relationships in a single query
        var roomWithRelationships = await _dbSet
            .Where(r => r.Id == id && r.TenantId == tenantId)
            .Select(r => new
            {
                Room = r,
                HasSchedules = r.Schedules.Any(),
                HasSessions = r.Sessions.Any()
            })
            .FirstOrDefaultAsync();

        if (roomWithRelationships == null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableRoom);

        // Check if room has child relationships
        if (roomWithRelationships.HasSchedules)
            return Result<bool>.Failure(_localizedStrings.RoomHasSchedules);

        if (roomWithRelationships.HasSessions)
            return Result<bool>.Failure(_localizedStrings.RoomHasSessions);

        // If no child relationships exist, proceed with deletion
        HardDelete(roomWithRelationships.Room);
        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckDuplicateRoomNameAsync(Room room, Guid tenantId) 
    {
        var existRoom = await _dbSet.AnyAsync(r => r.Name == room.Name && r.TenantId == tenantId);

        // Check if room name is duplicated
        if (existRoom)
            return Result<bool>.Failure(_localizedStrings.DuplicatedRoom);

        return Result<bool>.Success(true);
    }

}