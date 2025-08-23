namespace Tahil.Application.Rooms.Queries;

public record GetAllRoomsQuery() : IQuery<Result<List<LookupDto>>>;

public class GetAllRoomsQueryHandler(ILookupRepository lookupRepository, IApplicationContext applicationContext) : IQueryHandler<GetAllRoomsQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await lookupRepository.GetRoomsAsync(applicationContext.TenantId);
    }
}