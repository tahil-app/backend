namespace Tahil.Application.Teachers.Queries;

public record GetTeacherQuery(int Id) : IQuery<Result<TeacherDto>>;

public class GetTeacherQueryHandler(ITeacherRepository teacherRepository) : IQueryHandler<GetTeacherQuery, Result<TeacherDto>>
{
    public async Task<Result<TeacherDto>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
    {
        var teacherDto = await teacherRepository.GetTeacherAsync(request.Id);
        return Result.Success(teacherDto);
    }
}