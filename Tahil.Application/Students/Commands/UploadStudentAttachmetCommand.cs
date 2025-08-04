using Tahil.Application.Models;

namespace Tahil.Application.Students.Commands;

public record UploadStudentAttachmetCommand(UserAttachmentModel AttachmentModel) : ICommand<Result<bool>>;

public class UploadStudentAttachmetCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IAttachmentRepository attachmentRepository,
    IApplicationContext applicationContext,
    IStudentRepository studentRepository,
    LocalizedStrings locale)
    : ICommandHandler<UploadStudentAttachmetCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadStudentAttachmetCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.AttachmentModel.UserId, [r => r.StudentAttachments]);
        if (student is null)
            return Result<bool>.Failure(locale.NotAvailableStudent);

        var file = request.AttachmentModel.File;
        Guid guid = Guid.NewGuid();
        using var stream = file.OpenReadStream();
        var uploadFolderPath = Path.Combine("wwwroot", "attachments", "students", request.AttachmentModel.UserId.ToString());

        var uploadFileName = await uploadService.UploadAsync(uploadFolderPath, guid, file.FileName, stream);

        var attachment = attachmentRepository.AddAttachment(new AttachmentDto
        {
            FileName = uploadFileName,
            FileSize = stream.Length,
        }, applicationContext.UserName, applicationContext.TenantId);

        student.AddAttachment(attachment, request.AttachmentModel.DisplayName);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }

}