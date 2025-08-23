namespace Tahil.Application.Teachers.Queries;

public record GetCourseTeachersQueryQuery(int CourseId) : IQuery<Result<List<LookupDto>>>;

public class GetCourseTeachersQueryQueryHandler(ITeacherRepository teacherRepository, IApplicationContext applicationContext) : IQueryHandler<GetCourseTeachersQueryQuery, Result<List<LookupDto>>>
{
    public async Task<Result<List<LookupDto>>> Handle(GetCourseTeachersQueryQuery request, CancellationToken cancellationToken)
    {
        var teachers = await teacherRepository.GetAllReadOnlyProjectionAsyncAsync(
            r => new LookupDto
            {
                Id = r.Id,
                Name = r.User.Name
            },
            r => r.User.TenantId == applicationContext.TenantId && r.TeacherCourses.Any(tc => tc.CourseId == request.CourseId));

        return Result.Success(teachers);
    }
}