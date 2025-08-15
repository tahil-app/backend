namespace Tahil.Application.ClassSessions.Queries;

public record GetUserSessionsQuery(SessionSearchCriteriaDto sessionSearchCriteria) : IQuery<Result<List<ClassSessionDto>>>;

public class GetUserSessionsQueryHandler(IClassSessionRepository classSessionRepository) : IQueryHandler<GetUserSessionsQuery, Result<List<ClassSessionDto>>>
{

    public async Task<Result<List<ClassSessionDto>>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
    {
        return await classSessionRepository.GetClassSessionsAsync(request.sessionSearchCriteria);
    }

}