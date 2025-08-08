namespace Tahil.Application.Students.Queries;

public record GetStudentQuery(int Id) : IQuery<Result<StudentDto>>;

public class GetStudentQueryHandler(IStudentRepository studentRepository, IApplicationContext applicationContext) : IQueryHandler<GetStudentQuery, Result<StudentDto>>
{
    public async Task<Result<StudentDto>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var studentDto = await studentRepository.GetStudentAsync(request.Id, applicationContext.TenantId);
        return Result.Success(studentDto);
    }
}