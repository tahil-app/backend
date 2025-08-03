using Tahil.Application.Models;

namespace Tahil.Application.Teachers.Commands;

public record UploadTeacherAttachmetCommand(UserAttachmentModel AttachmentModel) : ICommand<Result<bool>>;

public class UploadTeacherAttachmetCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IAttachmentRepository attachmentRepository,
    IApplicationContext applicationContext,
    ITeacherRepository teacherRepository)
    : ICommandHandler<UploadTeacherAttachmetCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadTeacherAttachmetCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.AttachmentModel.UserId, [r => r.TeacherAttachments]);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        var file = request.AttachmentModel.File;
        Guid guid = Guid.NewGuid();
        using var stream = file.OpenReadStream();
        var uploadFolderPath = Path.Combine("wwwroot", "attachments", "teachers", request.AttachmentModel.UserId.ToString());

        var uploadFileName = await uploadService.UploadAsync(uploadFolderPath, guid, file.FileName, stream);

        var attachment = attachmentRepository.AddAttachment(new AttachmentDto
        {
            FileName = uploadFileName,
            FileSize = stream.Length,
        }, applicationContext.UserName, applicationContext.TenantId);

        teacher.AddAttachment(attachment, request.AttachmentModel.DisplayName);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }

}