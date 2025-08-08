using Tahil.Application.Teachers.Validators;
using Tahil.Domain.Entities;

namespace Tahil.Application.Teachers.Commands;

public record UpdateTeacherCommand(TeacherDto Teacher) : ICommand<Result<bool>>, ITeacherCommand;
public class UpdateTeacherCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository teacherRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<UpdateTeacherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await teacherRepository.UpdateTeacherAsync(request.Teacher, applicationContext.TenantId);
        if (updateResult.IsSuccess) 
        {
            var result = await unitOfWork.SaveChangesAsync();
            return Result.Success(true);
        }

        return updateResult;
    }
}