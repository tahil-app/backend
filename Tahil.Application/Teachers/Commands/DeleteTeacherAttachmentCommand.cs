namespace Tahil.Application.Teachers.Commands;

public record DeleteTeacherAttachmentCommand(int AttachmentId) : ICommand<Result<bool>>;

public class DeleteTeacherAttachmentCommandHandler(
    IUnitOfWork unitOfWork, 
    IUploadService uploadService, 
    ITeacherRepository teacherRepository,
    IAttachmentRepository attachmentRepository) : ICommandHandler<DeleteTeacherAttachmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTeacherAttachmentCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.TeacherAttachments.Any(a => a.AttachmentId == request.AttachmentId), [r => r.TeacherAttachments]);
        if (teacher is null)
            throw new NotFoundException("Attachment");

        teacher.RemoveTeacherAttachment(request.AttachmentId);
        
        var deletedAttach = await attachmentRepository.RemoveAttachment(request.AttachmentId);

        var result = await unitOfWork.SaveChangesAsync();
        if (result) 
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "teachers", teacher.Id.ToString(), deletedAttach.FileName!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }
}