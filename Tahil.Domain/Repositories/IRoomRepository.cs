namespace Tahil.Domain.Repositories;

public interface IRoomRepository : IRepository<Room> 
{
    Task<Result<bool>> AddRoomAsync(Room room, Guid tenantId);
    Task<Result<bool>> DeleteRoomAsync(int id);
}