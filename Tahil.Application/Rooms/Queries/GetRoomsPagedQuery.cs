namespace Tahil.Application.Rooms.Queries;

public record GetRoomsPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<RoomDto>>>;

public class GetRoomsPagedQueryHandler(IRoomRepository roomRepository) : IQueryHandler<GetRoomsPagedQuery, Result<PagedList<RoomDto>>>
{
    public async Task<Result<PagedList<RoomDto>>> Handle(GetRoomsPagedQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetPagedAsync(request.QueryParams);
        var pagedRooms = rooms.Adapt<PagedList<RoomDto>>();

        return Result<PagedList<RoomDto>>.Success(pagedRooms);
    }
}