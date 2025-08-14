namespace Tahil.Application.ClassSessions.Queries;

public record GetUserSessionsQuery : IQuery<Result<List<ClassSessionDto>>>
{
}

public class GetUserSessionsQueryHandler : IQueryHandler<GetUserSessionsQuery, Result<List<ClassSessionDto>>>
{
    private readonly IClassSessionRepository _classSessionRepository;

    public GetUserSessionsQueryHandler(IClassSessionRepository classSessionRepository)
    {
        _classSessionRepository = classSessionRepository;
    }

    public async Task<Result<List<ClassSessionDto>>> Handle(GetUserSessionsQuery request, CancellationToken cancellationToken)
    {
        return await _classSessionRepository.GetClassSessionsAsync();
    }

}