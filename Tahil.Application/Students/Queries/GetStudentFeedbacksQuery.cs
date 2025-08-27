namespace Tahil.Application.Students.Queries;


public record GetStudentFeedbacksQuery(int Id, int Year, int Month) : IQuery<Result<List<FeedbackDto>>>;

public class GetStudentFeedbacksQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentFeedbacksQuery, Result<List<FeedbackDto>>>
{
    public async Task<Result<List<FeedbackDto>>> Handle(GetStudentFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var studentFeedbacks = await studentRepository.GetStudentFeedbacksAsync(request.Id, request.Year, request.Month, applicationContext.TenantId);
        return Result.Success(studentFeedbacks);
    }
}
