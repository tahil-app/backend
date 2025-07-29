namespace Tahil.Application.Students.Queries;

public record GetStudentQuery(int Id) : IQuery<Result<StudentDto>>;

public class GetStudentQueryHandler(IStudentRepository studentRepository) : IQueryHandler<GetStudentQuery, Result<StudentDto>>
{
    public async Task<Result<StudentDto>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var studentDto = await studentRepository.GetStudentAsync(request.Id);
        return Result.Success(studentDto);
    }
}