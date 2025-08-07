namespace Tahil.Application.Students.Queries;

public record GetAllStudentsQuery() : IQuery<Result<List<StudentDto>>>;

public class GetAllStudentsQueryHandler(IStudentRepository studentRepository) : IQueryHandler<GetAllStudentsQuery, Result<List<StudentDto>>>
{
    public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetAllReadOnlyAsync(r => r.User.IsActive);
        var activeStudents = students.Adapt<List<StudentDto>>();

        return Result.Success(activeStudents);
    }
}