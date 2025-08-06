namespace Tahil.Application.Teachers.Queries;

public record GetCourseTeachersQueryQuery(int CourseId) : IQuery<Result<List<TeacherDto>>>;

public class GetCourseTeachersQueryQueryHandler(ITeacherRepository teacherRepository) : IQueryHandler<GetCourseTeachersQueryQuery, Result<List<TeacherDto>>>
{
    public async Task<Result<List<TeacherDto>>> Handle(GetCourseTeachersQueryQuery request, CancellationToken cancellationToken)
    {
        var teachers = await teacherRepository.GetAllReadOnlyAsync(r => r.TeacherCourses.Any(tc => tc.CourseId == request.CourseId));
        var pagedTeachers = teachers.Adapt<List<TeacherDto>>();

        return Result.Success(pagedTeachers);
    }
}