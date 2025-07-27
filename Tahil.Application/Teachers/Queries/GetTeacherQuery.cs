namespace Tahil.Application.Teachers.Queries;

public record GetTeacherQuery(int Id) : IQuery<Result<TeacherDto>>;

public class GetTeacherQueryHandler(ITeacherRepository teacherRepository) : IQueryHandler<GetTeacherQuery, Result<TeacherDto>>
{
    public async Task<Result<TeacherDto>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Id);
        var teacherDto = teacher.Adapt<TeacherDto>();

        return Result<TeacherDto>.Success(teacherDto);
    }
}