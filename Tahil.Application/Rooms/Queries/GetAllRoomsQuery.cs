namespace Tahil.Application.Rooms.Queries;

public record GetAllRoomsQuery() : IQuery<Result<List<RoomDto>>>;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllRoomsQuery, Result<List<RoomDto>>>
{
    public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllReadOnlyAsync(r => r.IsActive && r.TenantId == applicationContext.TenantId);
        var activeRooms = rooms.Adapt<List<RoomDto>>();

        return Result.Success(activeRooms);
    }
}