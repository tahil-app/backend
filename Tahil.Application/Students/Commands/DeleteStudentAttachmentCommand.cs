namespace Tahil.Application.Students.Commands;

public record DeleteStudentAttachmentCommand(int AttachmentId) : ICommand<Result<bool>>;

public class DeleteStudentAttachmentCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IStudentRepository studentRepository,
    IAttachmentRepository attachmentRepository,
    IApplicationContext applicationContext,
    LocalizedStrings locale) : ICommandHandler<DeleteStudentAttachmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteStudentAttachmentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.User.TenantId == applicationContext.TenantId && 
            r.StudentAttachments.Any(a => a.AttachmentId == request.AttachmentId), [r => r.StudentAttachments]);
        if (student is null)
            return Result<bool>.Failure(locale.NotAvailableAttachment);

        student.RemoveStudentAttachment(request.AttachmentId);

        var deletedAttach = await attachmentRepository.RemoveAttachment(request.AttachmentId, applicationContext.TenantId);

        var result = await unitOfWork.SaveChangesAsync();
        if (result)
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "students", student.Id.ToString(), deletedAttach.Value.FileName!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }
}