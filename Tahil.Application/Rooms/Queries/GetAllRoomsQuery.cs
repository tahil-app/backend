namespace Tahil.Application.Rooms.Queries;

public record GetAllRoomsQuery() : IQuery<Result<List<RoomDto>>>;

public class GetAllRoomsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllRoomsQuery, Result<List<RoomDto>>>
{
    public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetRoomsAsync(applicationContext.TenantId);
    }
}