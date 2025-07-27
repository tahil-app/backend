namespace Tahil.Domain.Repositories;

public interface IRoomRepository : IRepository<Room> 
{
    Task AddRoomAsync(Room room);
}