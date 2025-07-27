using Tahil.Application.Teachers.Models;

namespace Tahil.Application.Teachers.Commands;

public record UploadTeacherAttachmetCommand(TeacherAttachmentModel Model) : ICommand<Result<bool>>;

public class UploadTeacherAttachmetCommandHandler(IUnitOfWork unitOfWork, IAttachmentService attachmentService, ITeacherRepository teacherRepository)
    : ICommandHandler<UploadTeacherAttachmetCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadTeacherAttachmetCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.Model.TeacherId, [r => r.TeacherAttachments]);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        var uploadsFolder = Path.Combine("wwwroot", "attachments", "teachers", request.Model.TeacherId.ToString());

        using var stream = request.Model.File.OpenReadStream();
        var attachment = await attachmentService.AddAsync(uploadsFolder, request.Model.File.FileName, request.Model.Title, stream);

        teacher.AddTeacherAttachment(attachment);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }

}