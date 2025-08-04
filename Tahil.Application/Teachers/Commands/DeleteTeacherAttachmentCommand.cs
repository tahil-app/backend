namespace Tahil.Application.Teachers.Commands;

public record DeleteTeacherAttachmentCommand(int AttachmentId) : ICommand<Result<bool>>;

public class DeleteTeacherAttachmentCommandHandler(
    IUnitOfWork unitOfWork, 
    IUploadService uploadService, 
    ITeacherRepository teacherRepository,
    IAttachmentRepository attachmentRepository,
    LocalizedStrings locale) : ICommandHandler<DeleteTeacherAttachmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTeacherAttachmentCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.TeacherAttachments.Any(a => a.AttachmentId == request.AttachmentId), [r => r.TeacherAttachments]);
        if (teacher is null)
            return Result<bool>.Failure(locale.NotAvailableAttachment);

        teacher.RemoveTeacherAttachment(request.AttachmentId);
        
        var deletedAttach = await attachmentRepository.RemoveAttachment(request.AttachmentId);

        var result = await unitOfWork.SaveChangesAsync();
        if (result) 
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "teachers", teacher.Id.ToString(), deletedAttach.Value.FileName!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }
}